using static MathExtensions;

static partial class Aoc2024
{
    public static void Day9()
    {
        var day = MethodBase.GetCurrentMethod()!.Name;

        ComputeExample(); Compute();

        void ComputeExample()
        {
            var input = "2333133121414131402";
            Part1(input).Should().Be(1928);
            Part2(input).Should().Be(0);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt")[0];
            Part1(input).Should().Be(6384282079460);
            Part2(input).Should().Be(0);
        }

        long Part1(string line) => Checksum(Compact(Parse(line)));
        int Part2(string line) => 0;

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
            var file = diskmap.Last!;
            var emptySpace = diskmap.First!.Next!;

            while (file != null && emptySpace != null)
            {
                if (emptySpace.Value.space > file.Value.space)
                {
                    // file fits in empty space, move it
                    var nextFile = NextFile(file);
                    diskmap.Remove(file);
                    diskmap.AddBefore(emptySpace, file);

                    // update empty space
                    emptySpace.Value = (-1, emptySpace.Value.space - file.Value.space);
                    
                    // next file
                    file = nextFile;
                }
                else if (emptySpace.Value.space == file.Value.space)
                {
                    // file fits in empty space, move it
                    var nextFile = NextFile(file);
                    diskmap.Remove(file);
                    diskmap.AddBefore(emptySpace, file);

                    // remove empty space
                    var nextEmptySpace = NextSpace(emptySpace);
                    diskmap.Remove(emptySpace);
                    emptySpace = nextEmptySpace;
                    
                    // next file
                    file = nextFile;
                }
                else
                {
                    // update remaining file space
                    file.Value = (file.Value.id, file.Value.space - emptySpace.Value.space);

                    // insert file space
                    diskmap.AddBefore(emptySpace, (file.Value.id, emptySpace.Value.space));

                    // remove empty space
                    var nextEmptySpace = NextSpace(emptySpace);
                    diskmap.Remove(emptySpace);
                    emptySpace = nextEmptySpace;
                }

                if (IsLastFile(file))
                {
                    break;
                }
            }

            return diskmap;

            static LinkedListNode<(int id, int space)>? NextFile(LinkedListNode<(int id, int space)>? node)
            {
                node = node?.Previous;
                while (node != null && node.Value.id == -1)
                {
                    node = node.Previous;
                }
                return node;
            }
            static LinkedListNode<(int id, int space)>? NextSpace(LinkedListNode<(int id, int space)>? node)
            {
                node = node?.Next;
                while (node != null && node.Value.id != -1)
                {
                    node = node.Next;
                }
                
                return node;
            }
            static bool IsLastFile(LinkedListNode<(int id, int space)>? node)
            {
                node = node?.Previous;
                while (node != null && node.Value.id != -1)
                {
                    node = node.Previous;
                }
                
                return node == null;
            }
        }

        static LinkedList<(int id, int space)> Parse(string line) => 
            new (line
            .Select((c, i) => (id: i % 2 == 0 ? i / 2 : -1, space: c.ToInt()))
            .Where(x => x.space > 0));
    }
}