using System;

/// <summary>
///  always go smaller first to bigger second
/// </summary>
public class RunIndexingProgram
{
	static public void Main(string[] args)
	{
		string filepath = "C:/Users/juzhu/Desktop/Alignment/";
		string[] filenames = Directory.GetFiles(filepath);
		if (filenames.Length == 0)
        {
			throw new Exception("directory empty or non existent");
        }

		ChromatogramRetTimes[] master_index = new ChromatogramRetTimes[filenames.Length*2]; ///handle filename being null
		for (int i=0; i<filenames.Length; i++)
		{
			if (File.Exists(filenames[i]))
            {
				var two_chromos = GetPairedTimes(filenames[i]); ///master index adding
				master_index[i*2] = two_chromos[0];
				master_index[i*2+1] = two_chromos[1];
			}
            else
            {
				throw new Exception("file doesn't exist");
            }
		}
		double mem_size = GC.GetTotalMemory(true);
		mem_size /= 1024;
		mem_size /= 1024;
		Console.WriteLine("success!\nMemory usage in mb: {0}\n", mem_size);		
	}

	static public ChromatogramRetTimes[] GetPairedTimes(string filename)
    {
		int counter = 0;
		List<float> buffer1 = new(); 
		List<float> buffer2 = new();

		foreach (var time_str in File.ReadLines(filename))
        {
			float time_float = float.Parse(time_str);
			if (counter % 2 == 0) { buffer1.Add(time_float); }
			else { buffer2.Add(time_float); }
			counter += 1;
		}

		string[] whichChromo = filename.Split("_");
		string chromo1name = whichChromo[^5];
		string chromo2name = whichChromo[^1][..^4];

		ChromatogramRetTimes retTimes1 = new(chromo1name, buffer1.ToArray());
		ChromatogramRetTimes retTimes2 = new(chromo2name, buffer2.ToArray());

		ChromatogramRetTimes[] returnArray = { retTimes1, retTimes2 }; 
		return returnArray;
	}
}
