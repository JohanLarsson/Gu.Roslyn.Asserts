namespace Gu.Roslyn.Asserts.Analyzers
{
    using System;
    using System.Collections.Immutable;
    using System.Diagnostics;
    using System.Threading;
    using Gu.Roslyn.AnalyzerExtensions;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    [DebuggerDisplay("{Expression}")]
    internal struct ArgumentInfo : IEquatable<ArgumentInfo>
    {
        internal readonly ArgumentSyntax Argument;
        internal readonly IParameterSymbol Parameter;
        internal readonly ExpressionSyntax Expression;
#pragma warning disable RS1008 // Avoid storing per-compilation data into the fields of a diagnostic analyzer.
        internal readonly ISymbol Symbol;
#pragma warning restore RS1008 // Avoid storing per-compilation data into the fields of a diagnostic analyzer.
        internal readonly ExpressionSyntax Value;

        private ArgumentInfo(ArgumentSyntax argument, IParameterSymbol parameter, ExpressionSyntax identifierName, ISymbol symbol, ExpressionSyntax value)
        {
            this.Argument = argument;
            this.Parameter = parameter;
            this.Expression = identifierName;
            this.Symbol = symbol;
            this.Value = value;
        }

        internal bool? HasPosition
        {
            get
            {
                switch (this.Value)
                {
                    case LiteralExpressionSyntax literal when literal.IsKind(SyntaxKind.StringLiteralExpression):
                        return literal.Token.ValueText.Contains("↓");
                    case InvocationExpressionSyntax invocation when invocation.Expression is MemberAccessExpressionSyntax memberAccess &&
                                                                    memberAccess.Expression is LiteralExpressionSyntax literal &&
                                                                    literal.Token.ValueText.Contains("↓"):
                        return true;
                    default:
                        return null;
                }
            }
        }

        public static bool operator ==(ArgumentInfo left, ArgumentInfo right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ArgumentInfo left, ArgumentInfo right)
        {
            return !left.Equals(right);
        }

        public bool Equals(ArgumentInfo other) => this.Expression.Equals(other.Expression);

        public override bool Equals(object obj) => obj is ArgumentInfo other && this.Equals(other);

        public override int GetHashCode() => this.Expression.GetHashCode();

        internal static ImmutableArray<ArgumentInfo> CreateMany(ArgumentSyntax argument, IParameterSymbol parameter, SemanticModel semanticModel, CancellationToken cancellationToken)
        {
            if (TryGetInitializer(out var initializer))
            {
                var builder = ImmutableArray.CreateBuilder<ArgumentInfo>(initializer.Expressions.Count);
                foreach (var expression in initializer.Expressions)
                {
                    builder.Add(Create(argument, parameter, expression, semanticModel, cancellationToken));
                }

                return builder.MoveToImmutable();
            }

            if (parameter.IsParams)
            {
                if (argument.Parent is ArgumentListSyntax argumentList)
                {
                    var builder = ImmutableArray.CreateBuilder<ArgumentInfo>(argumentList.Arguments.Count - parameter.Ordinal);
                    for (var i = parameter.Ordinal; i < argumentList.Arguments.Count; i++)
                    {
                        builder.Add(Create(argument, parameter, argumentList.Arguments[i].Expression, semanticModel, cancellationToken));
                    }

                    return builder.MoveToImmutable();
                }

                return ImmutableArray<ArgumentInfo>.Empty;
            }

            return ImmutableArray.Create(Create(argument, parameter, argument.Expression, semanticModel, cancellationToken));

            bool TryGetInitializer(out InitializerExpressionSyntax result)
            {
                switch (argument.Expression)
                {
                    case ImplicitArrayCreationExpressionSyntax arrayCreation:
                        result = arrayCreation.Initializer;
                        return result != null;
                    case ArrayCreationExpressionSyntax arrayCreation:
                        result = arrayCreation.Initializer;
                        return result != null;
                    case ObjectCreationExpressionSyntax objectCreation:
                        result = objectCreation.Initializer;
                        return result != null;
                    default:
                        result = null;
                        return false;
                }
            }
        }

        private static ArgumentInfo Create(ArgumentSyntax argument, IParameterSymbol parameter, ExpressionSyntax expression, SemanticModel semanticModel, CancellationToken cancellationToken)
        {
            if (expression is IdentifierNameSyntax candidate &&
                semanticModel.TryGetSymbol(candidate, cancellationToken, out ISymbol candidateSymbol))
            {
                _ = TryGetValue(out var literal);
                return new ArgumentInfo(argument, parameter, expression, candidateSymbol, literal);
            }

            return new ArgumentInfo(argument, parameter, expression, null, null);

            bool TryGetValue(out ExpressionSyntax result)
            {
                if (candidateSymbol.TrySingleDeclaration(cancellationToken, out LocalDeclarationStatementSyntax localDeclaration) &&
                    localDeclaration.Declaration is VariableDeclarationSyntax localVariableDeclaration &&
                    localVariableDeclaration.Variables.TrySingle(out var localVariable) &&
                    localVariable.Initializer is EqualsValueClauseSyntax localInitializer)
                {
                    result = localInitializer.Value;
                    return true;
                }

                if (candidateSymbol.TrySingleDeclaration(cancellationToken, out FieldDeclarationSyntax fieldDeclaration) &&
                    fieldDeclaration.Declaration is VariableDeclarationSyntax fieldVariableDeclaration &&
                    fieldVariableDeclaration.Variables.TrySingle(out var fieldVariable) &&
                    fieldVariable.Initializer is EqualsValueClauseSyntax fieldInitializer)
                {
                    result = fieldInitializer.Value;
                    return true;
                }

                result = null;
                return false;
            }
        }
    }
}
