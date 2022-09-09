using System;
/// <summary>
/// chromas will be singular
/// bins are each minute, [inclusive, exclusive)
/// </summary>
/// 
/// <summary>
/// 
/// </summary>
/// <param name="rawtimes"></param>
/// <param name="upperbound"></param>
/// <exception cref="ArgumentException"></exception>
public class ChromatogramRetTimes
{
	public string name; 
	public float[][] retTimes;
	public ChromatogramRetTimes(string new_name, float[] rawtimes)
	{ ///upperbound value?
		name = new_name;
		float upperbound = rawtimes.Max();

		retTimes = new float[ (int)Math.Ceiling(upperbound) ][]; ///upperbound for number arrays
		Pre_Sanity_Checks(rawtimes, upperbound);
		IndexRawTimes(rawtimes);
		Post_Sanity_Checks(rawtimes.Length);
	}
	private void IndexRawTimes(float[] rawtimes)
	{
		float binUpperBound = 1;
		List<float> buffer = new(); ///equal spaces

		foreach (var time in rawtimes)
		{
			if (time >= binUpperBound)
			{
				retTimes[(int)binUpperBound - 1] = buffer.ToArray();
				buffer.Clear();
				int gap = (int)(time - binUpperBound); ///find array gap difference

				for (int i = 0; i < gap; i++)
				{
					retTimes[i + (int)binUpperBound] = Array.Empty<float>();
				}

				binUpperBound += (gap + 1);
			}
			buffer.Add(time);
		}
		retTimes[(int)binUpperBound - 1] = buffer.ToArray(); ///add last array
	}
	private void Pre_Sanity_Checks(float[] raw_times, float upperbound) ///check size constraints
	{
		if (raw_times != null)
        {
			if(raw_times.Length > 0)
            {
				if (raw_times.Max() > upperbound && raw_times.Min() < 0) ///check negative values and max value 
                {
					string err_message = String.Format("range issue: \n Max: {0}\n Min: {1}", raw_times.Max(), raw_times.Min());
					throw new Exception(err_message); //the max can be a little high, set with movable upper bound next time
				}
			}
			else
			{
				throw new Exception("length issue");
			}
		}
        else
        {
			throw new Exception("null issue");
        }
	}
	private void Post_Sanity_Checks(int rawtimes_length) ///double check later
	{
		if (retTimes.Length > 121 || retTimes.Length < 120) ///length is managable
		{
			throw new Exception("test 1 fail");
		}

		for (int i = 0; i < retTimes.Length; i++)
		{
			int arr_length = retTimes[i].Length;
			float[] sorted = new float[arr_length];///checking inner array is in order

			Array.Copy(retTimes[i], sorted, arr_length);
			Array.Sort(sorted);
			
			if (!Enumerable.SequenceEqual(sorted, retTimes[i]))
			{
				throw new Exception("test 2 fail");
			}
		}

		List<float> end_values = new(); ///checking outer array is in order and Total number of entries are same
		int counter = 0;
		for (int i = 0; i < retTimes.Length; i++)
		{
			if (retTimes[i].Count() > 0)
			{
				end_values.Add(retTimes[i].Min());
				end_values.Add(retTimes[i].Max());
			}
			counter += retTimes[i].Count();
		}

		float[] sorted_ends = end_values.ToArray();
		Array.Sort(sorted_ends);
		if (!Enumerable.SequenceEqual(sorted_ends, end_values))
		{
			throw new Exception("test 3 fail");
		}
		if (counter != rawtimes_length)
        {
			throw new Exception("test 4 fail");
        }
	}
}
