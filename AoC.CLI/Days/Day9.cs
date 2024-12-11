using System.Text;

namespace AoC.CLI.Days;

public class Day9
    : IDay
{
    public Task<string> ExecutePart1(
        IEnumerable<string> input)
    {
        var answer = input
            .CreateDiskMap()
            .GenerateBlocks()
            .Defragment()
            .CalculateChecksum()
            .ToString();

        return Task.FromResult(answer);
    }

    public Task<string> ExecutePart2(
        IEnumerable<string> input)
    {
        var answer = input
            .CreateDiskMap()
            .GenerateStorage()
            .Defragment()
            .ToFileSystemString()
            .CalculateChecksum()
            .ToString();

        return Task.FromResult(answer);
    }
}

internal static class Constants
{
    public const int FreeSpaceFileId = -1;
}

internal static class Day9Extensions
{
    public static List<int> CreateDiskMap(
        this IEnumerable<string> input)
    {
        var diskMap = input
            .First()
            .Select(x => (int)Char.GetNumericValue(x))
            .ToList();

        return diskMap;
    }

    public static List<Block> GenerateStorage(
        this List<int> diskMap)
    {
        var blocks = new List<Block>();
        var fileId = 0;

        for(var i = 0; i < diskMap.Count; i++)
        {
            var size = diskMap[i];

            if (size == 0)
            {
                continue;
            }

            var block = new Block(
                i % 2 == 0 ? fileId++ : Constants.FreeSpaceFileId,
                size);

            blocks
                .Add(block);
        }

        return blocks;
    }

    public static List<Block> Defragment(
        this List<Block> storage)
    {
        List<Block> defragmented = [..storage];

        var files = storage
            .Where(x => x.FileId != Constants.FreeSpaceFileId)
            .Reverse();

        foreach (var file in files)
        {
            var freeSpace = defragmented
                .FirstOrDefault(x => x.FileId == Constants.FreeSpaceFileId && x.Size >= file.Size);

            if (freeSpace is not null)
            {
                var rightIndex = defragmented.IndexOf(file);
                var leftIndex = defragmented.IndexOf(freeSpace);

                if (leftIndex >= rightIndex)
                {
                    continue;
                }

                (defragmented[rightIndex], defragmented[leftIndex]) = (defragmented[leftIndex], defragmented[rightIndex]);

                if (freeSpace.Size <= file.Size)
                {
                    continue;
                }

                var newFreeSpaceSize = freeSpace.Size - file.Size;

                defragmented[rightIndex] = defragmented[rightIndex] with { Size = file.Size };

                var newFreeSpace = new Block(Constants.FreeSpaceFileId, newFreeSpaceSize);
                defragmented
                    .Insert(leftIndex + 1, newFreeSpace);

                defragmented = defragmented
                    .ConsolidateFreeSpace();
            }
        }

        return defragmented;
    }

    private static List<Block> ConsolidateFreeSpace(
        this List<Block> storage)
    {
        List<Block> consolidated = [..storage];

        for (var i = 0; i < consolidated.Count; i++)
        {
            if (i > 0)
            {
                consolidated.ConsolidateFreeSpace(
                    i - 1,
                    i);
            }

            if (i < consolidated.Count - 1)
            {
                consolidated.ConsolidateFreeSpace(
                    i,
                    i + 1);
            }
        }

        return consolidated;
    }

    private static void ConsolidateFreeSpace(
        this List<Block> storage,
        int leftIndex,
        int rightIndex)
    {
        var leftBlock = storage[leftIndex];
        var rightBlock = storage[rightIndex];

        if (leftBlock.FileId != Constants.FreeSpaceFileId || rightBlock.FileId != Constants.FreeSpaceFileId)
        {
            return;
        }

        storage[leftIndex] = leftBlock with { Size = leftBlock.Size + rightBlock.Size };
        storage
            .RemoveAt(rightIndex);
    }

    public static List<string> ToFileSystemString(
        this List<Block> storage)
    {
        return storage
            .Select(x => x.ToFileSystem())
            .SelectMany(x => x)
            .ToList();
    }

    public static List<string> GenerateBlocks(
        this List<int> diskMap)
    {
        var blocks = new List<string>();

        bool isFile = true;
        int fileId = 0;
        foreach (var digit in diskMap)
        {
            for (var i = 0; i < digit; i++)
            {
                var block = ".";

                if (isFile)
                {
                    block = fileId.ToString();
                }

                blocks.Add(block);
            }

            if (isFile)
            {
                fileId++;
            }
            isFile = !isFile;
        }

        return blocks;
    }

    public static List<string> Defragment(
        this List<string> fragmentedBlocks)
    {
        List<string> defragmentedBlocks = [..fragmentedBlocks];

        var lastFileIndex = defragmentedBlocks.Count - 1;

        for (var i = 0; i < defragmentedBlocks.Count; i++)
        {
            if (defragmentedBlocks[i] == ".")
            {
                for (var j = lastFileIndex; j >= 0; j--)
                {
                    if (defragmentedBlocks[j] != ".")
                    {
                        lastFileIndex = j;
                        break;
                    }
                }

                if (lastFileIndex <= i)
                {
                    break;
                }

                (defragmentedBlocks[i], defragmentedBlocks[lastFileIndex]) = (defragmentedBlocks[lastFileIndex], defragmentedBlocks[i]);
            }
        }

        return defragmentedBlocks;
    }

    public static List<string> DefragmentFiles(
        List<string> fragmentedBlocks,
        string lastFileId)
    {
        List<string> defragmentedBlocks = [..fragmentedBlocks];

        var fileIndexes = defragmentedBlocks
            .GetFileIndexes(lastFileId);

        var freeSpaceIndexes = defragmentedBlocks
            .GetFreeSpaceIndexes(fileIndexes.Count);

        var currentFile = defragmentedBlocks[fileIndexes[0]];

        if (!freeSpaceIndexes.Any())
        {
            return DefragmentFiles(defragmentedBlocks, currentFile);
        }

        if (freeSpaceIndexes.First() >= fileIndexes.First())
        {
            return defragmentedBlocks;
        }

        for (var i = 0; i < fileIndexes.Count; i++)
        {
            var freeSpace = freeSpaceIndexes[i];
            var file = fileIndexes[i];

            (defragmentedBlocks[freeSpace], defragmentedBlocks[file]) = (defragmentedBlocks[file], defragmentedBlocks[freeSpace]);
        }

        return DefragmentFiles(defragmentedBlocks, currentFile);
    }

    private static List<int> GetFileIndexes(
        this List<string> blocks,
        string lastFileId)
    {
        var indexes = new List<int>();
        var currentFile = string.Empty;
        var lastFilePassed = lastFileId == string.Empty;

        for (var i = blocks.Count - 1; i >= 0; i--)
        {
            if (blocks[i] == ".")
            {
                continue;
            }

            if (blocks[i] == lastFileId)
            {
                lastFilePassed = true;
                continue;
            }

            if (lastFilePassed)
            {
                if (currentFile == string.Empty || currentFile == blocks[i])
                {
                    currentFile = blocks[i];
                    indexes.Add(i);
                }

                if (currentFile != blocks[i])
                {
                    break;
                }
            }
        }

        return indexes;
    }

    private static List<int> GetFreeSpaceIndexes(
        this List<string> blocks,
        int fileSize)
    {
        var freeSpaceIndexes = new List<int>();

        for (var i = 0; i <= blocks.Count - 1; i++)
        {
            if (blocks[i] != ".")
            {
                freeSpaceIndexes = [];
                continue;
            }

            if (blocks[i] == ".")
            {
                freeSpaceIndexes.Add(i);
            }

            if (freeSpaceIndexes.Count == fileSize)
            {
                break;
            }
        }

        return freeSpaceIndexes.Count == fileSize ?
            freeSpaceIndexes :
            [];
    }

    public static long CalculateChecksum(
        this List<string> blocks)
    {
        long sum = 0;
        for (var i = 0; i < blocks.Count; i++)
        {
            if (blocks[i] == ".")
            {
                continue;
            }

            sum += int.Parse(blocks[i]) * i;
        }

        return sum;
    }
}

internal record Block(
    int FileId,
    int Size)
{
    public List<string> ToFileSystem()
    {
        var fileSystem = new List<string>();

        for (var i = 0; i < Size; i++)
        {
            fileSystem.Add(FileId == -1 ? "." : FileId.ToString());
        }

        return fileSystem;
    }
};