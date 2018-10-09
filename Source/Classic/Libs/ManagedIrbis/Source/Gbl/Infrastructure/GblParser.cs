// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* GblParser.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Gbl.Infrastructure.Ast;

using MoonSharp.Interpreter;

#endregion

// ReSharper disable StringLiteralTypo

namespace ManagedIrbis.Gbl.Infrastructure
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class GblParser
    {
        #region Privare members

        [NotNull]
        private GblNode[] _CollectNodes
            (
                [NotNull] IEnumerable<string> text
            )
        {
            List<GblNode> nodes = new List<GblNode>();
            using (IEnumerator<string> lines = text.GetEnumerator())
            {
                while (true)
                {
                    string first = lines.GetNextItem();
                    string second = lines.GetNextItem();
                    string third = lines.GetNextItem();
                    string fourth = lines.GetNextItem();
                    string fifth = lines.GetNextItem();
                    if (ReferenceEquals(first, null)
                        || ReferenceEquals(second, null)
                        || ReferenceEquals(third, null)
                        || ReferenceEquals(fourth, null)
                        || ReferenceEquals(fifth, null))
                    {
                        break;
                    }

                    first = first.Trim().ToUpperInvariant();
                    if (string.IsNullOrEmpty(first))
                    {
                        break;
                    }

                    GblNode node;
                    switch (first)
                    {
                        case "//":
                        case "///":
                            node = new GblNop();
                            break;

                        case "ADD":
                            node = new GblAdd();
                            break;

                        case "ALL":
                            node = new GblAll();
                            break;

                        case "CHA":
                            node = new GblCha();
                            break;

                        case "CHAC":
                            node = new GblChac();
                            break;

                        case "CONTEXTIN":
                            node = new GblContextIn();
                            break;

                        case "CONTEXTOUT":
                            node = new GblContextOut();
                            break;

                        case "CORREC":
                            node = new GblCorrec();
                            break;

                        case "DEL":
                            node = new GblDel();
                            break;

                        case "DELR":
                            node = new GblDelr();
                            break;

                        case "EMPTY":
                            node = new GblEmpty();
                            break;

                        case "END":
                            node = new GblInternal("END");
                            break;

                        case "FI":
                            node = new GblInternal("FI");
                            break;

                        case "IF":
                            node = new GblIf();
                            break;

                        case "NEWMFN":
                            node = new GblNewMfn();
                            break;

                        case "PUTLOG":
                            node = new GblPutLog();
                            break;

                        case "REP":
                            node = new GblRep();
                            break;

                        case "REFRESHDB":
                            node = new GblRefreshDb();
                            break;

                        case "REPEAT":
                            node = new GblRepeat();
                            break;

                        case "UNDELR":
                            node = new GblUndel();
                            break;

                        case "UNDOR":
                            node = new GblUndor();
                            break;

                        case "UNTIL":
                            node = new GblInternal("UNTIL");
                            break;

                        default:
                            if (first[0] == '@')
                            {
                                node = new GblNested();
                                break;
                            }

                            throw new IrbisException();
                    }

                    node.Parameter1 = second;
                    node.Parameter2 = third;
                    node.Format1 = fourth;
                    node.Format2 = fifth;
                    nodes.Add(node);
                }
            }

            return nodes.ToArray();
        }

        private int _FindClosingNode
            (
                [NotNull] GblNode[] nodes,
                int startIndex,
                [NotNull] string nodeKind
            )
        {
            // TODO handle nested
            for (int j = startIndex + 1; j < nodes.Length; j++)
            {
                GblNode node = nodes[j];
                GblInternal special = node as GblInternal;
                if (!ReferenceEquals(special, null))
                {
                    if (special.Command.SameString(nodeKind))
                    {
                        return j;
                    }
                }
            }

            return -1;
        }

        [NotNull]
        private GblNode[] _ArrangeNodes
            (
                [NotNull] GblNode[] nodes
            )
        {
            List<GblNode> result = new List<GblNode>();
            for (int index = 0; index < nodes.Length; index++)
            {
                GblNode node = nodes[index];

                GblIf ifNode = node as GblIf;
                if (!ReferenceEquals(ifNode, null))
                {
                    int edge = _FindClosingNode(nodes, index + 1, "FI");
                    if (edge < 0)
                    {
                        throw new IrbisException();
                    }

                    ifNode.Children.AddRange(nodes.Segment(index + 1, edge - index - 2));
                    index = edge;
                }

                result.Add(node);
            }

            return result.ToArray();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the text.
        /// </summary>
        [NotNull]
        public GblProgram Parse
            (
                [NotNull] IEnumerable<string> text,
                [NotNull] GblContext context
            )
        {
            Code.NotNull(text, "text");
            Code.NotNull(context, "context");

            GblProgram result = new GblProgram();
            GblNode[] nodes = _CollectNodes(text);
            nodes = _ArrangeNodes(nodes);
            result.Nodes.AddRange(nodes);
            result.Initialize(context, this);

            return result;
        }

        #endregion
    }
}
