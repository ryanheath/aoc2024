﻿static partial class Aoc2024
{
    public static void Day9()
    {
        var day = MethodBase.GetCurrentMethod()!.Name;

        ComputeExample(); Compute();

        void ComputeExample()
        {
            var input = "2333133121414131402";
            Part1(input).Should().Be(1928);
            Part2(input).Should().Be(2858);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt")[0];
            Part1(input).Should().Be(6384282079460);
            Part2(input).Should().Be(6408966547049);
        }

        long Part1(string line) => Checksum(Compact(Parse(line)));
        long Part2(string line) => Checksum(Defrag(Parse(line)));

        static long Checksum(LinkedList<(int id, int space)> diskmap)
        {
            var i = 0;
            var sum = 0L;

            foreach (var n in diskmap)
            {
                if (n.id != -1)
                {
                    sum += SumOfN(i, (i + n.space - 1)) * n.id;
                }
                i += n.space;
            }

            return sum;
        }

        static LinkedList<(int id, int space)> Compact(LinkedList<(int id, int space)> diskmap)
        {
            var file = diskmap.Last;
            var emptySpace = diskmap.First!.Next;

            while (file != null && emptySpace != null)
            {
                if (emptySpace.Value.space >= file.Value.space)
                {
                    // file fits in empty space, move it
                    var nextFile = NextFile(file);
                    diskmap.Remove(file);
                    diskmap.AddBefore(emptySpace, file);

                    if (emptySpace.Value.space == file.Value.space)
                    {
                        // remove empty space
                        var nextEmptySpace = NextSpace(emptySpace);
                        diskmap.Remove(emptySpace);
                        emptySpace = nextEmptySpace;
                    }
                    else
                    {
                        // update empty space
                        emptySpace.Value = emptySpace.Value with { space = emptySpace.Value.space - file.Value.space };
                    }
                    
                    // next file
                    file = nextFile;
                }
                else
                {
                    // update remaining file space
                    file.Value = file.Value with { space = file.Value.space - emptySpace.Value.space };

                    // insert file space
                    diskmap.AddBefore(emptySpace, file.Value with { space = emptySpace.Value.space });

                    // remove empty space
                    var nextEmptySpace = NextSpace(emptySpace);
                    diskmap.Remove(emptySpace);
                    emptySpace = nextEmptySpace;
                }

                if (IsLastFileToMove(file))
                {
                    break;
                }
            }

            return diskmap;

            static LinkedListNode<(int id, int space)>? NextSpace(LinkedListNode<(int id, int space)>? node)
            {
                node = node?.Next;
                while (node != null && node.Value.id != -1)
                {
                    node = node.Next;
                }
                
                return node;
            }

            static LinkedListNode<(int id, int space)>? NextFile(LinkedListNode<(int id, int space)>? node)
            {
                node = node?.Previous;
                while (node != null && node.Value.id == -1) // skip space
                {
                    node = node.Previous;
                }
                
                return node;
            }
        }

        static LinkedList<(int id, int space)> Defrag(LinkedList<(int id, int space)> diskmap)
        {
            var filesToMove = new Queue<LinkedListNode<(int id, int space)>>();
            var file = diskmap.Last;
            while (file != null)
            {
                if (file.Value.id != -1)
                {
                    filesToMove.Enqueue(file);
                }

                file = file.Previous;
            }
            var firstEmptySpace = diskmap.First!.Next;

            while (filesToMove.Count > 0)
            {
                file = filesToMove.Dequeue();
                var emptySpace = NextSpace(file);

                if (emptySpace is not null)
                {
                    // add empty space at file
                    diskmap.AddBefore(file, file.Value with { id = -1 });
                    diskmap.Remove(file);
                    diskmap.AddBefore(emptySpace, file);

                    if (emptySpace.Value.space > file.Value.space)
                    {
                        // update empty space
                        emptySpace.Value = emptySpace.Value with { space = emptySpace.Value.space - file.Value.space };
                    }
                    else
                    {
                        if (firstEmptySpace == emptySpace)
                        {
                            firstEmptySpace = NextFirstSpace(firstEmptySpace);
                        }

                        // remove empty space
                        diskmap.Remove(emptySpace);
                    }
                }

                if (IsLastFileToMove(filesToMove.Peek()))
                {
                    break;
                }
            }

            return diskmap;

            LinkedListNode<(int id, int space)>? NextSpace(LinkedListNode<(int id, int space)>? file)
            {
                var node = firstEmptySpace;
                while (true)
                {
                    if (node == null || node == file)
                    {
                        return null;
                    }
                    else if (node.Value.id == -1 && node.Value.space >= file?.Value.space)
                    {
                        return node;
                    }
                    node = node.Next;
                }
            }

            LinkedListNode<(int id, int space)>? NextFirstSpace(LinkedListNode<(int id, int space)>? firstSpace)
            {
                var node = firstEmptySpace?.Next;
                while (node != null && node.Value.id != -1) // skip files
                {
                    node = node.Next;
                }
                return node;
            }
        }

        static bool IsLastFileToMove(LinkedListNode<(int id, int space)>? node)
        {
            node = node?.Previous;
            while (node != null && node.Value.id != -1) // skip files
            {
                node = node.Previous;
            }
            
            return node == null;
        }

        static LinkedList<(int id, int space)> Parse(string line) => 
            new (line
                    .Select((c, i) => (id: i % 2 == 0 ? i / 2 : -1, space: c.ToInt()))
                    .Where(x => x.space > 0));
    }
}