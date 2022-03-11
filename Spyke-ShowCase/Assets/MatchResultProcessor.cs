using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MatchResult
{
    public string[] Match;
    public int Period;
    public int[] FrequencyTable;
    public int[] MinRange;
    public int[] MaxRange;

    public string Id => string.Join(",",Match);

    public MatchResult(int[] table)
    {
        SetDistribution(table);
    }

    public void SetDistribution(int[] table)
    {
        FrequencyTable = table;
        MinRange = FrequencyTable.Select((x,i)=> GetRange(i)).ToArray();
        MaxRange = FrequencyTable.Select((x,i)=> GetRange(i + 1)-1).ToArray();
        Period = FrequencyTable.Count();
    }
    public void PrintBoundires()
    {
        var msg = $"{Id} % {Period}";
        for (int i = 0; i < FrequencyTable.Length; i++)
        {
            msg += $",[{MinRange[i]},{MaxRange[i]}]";
        }
        // Debug.Log(msg + $",Period %{Period}");
    }
    public int GetRange(int i)
    {
        return FrequencyTable.Take(i).Sum();
    }

    public int GetPeriodIndex(int index)
    {
        for (int i = 0; i < FrequencyTable.Length; i++)
        {
            var min = GetRange(i);
            var max = GetRange(i + 1);
            if(index>= min && index<max)
            {
                return  i;
            }
        }
        throw new MissingComponentException();
    }
}

public class MatchResultProcessor
{
    List<MatchResult> _resultGroup = new List<MatchResult>();
    Dictionary<int, MatchResult> _result = new Dictionary<int, MatchResult>();


    public MatchResultProcessor()
    {
        CreateResultGroups();
        // NewAlogrithm();
        CreateResult();
        //TryOut();
        
    }

    public void TryOut()
    {
        Dictionary<int,MatchResult> matchMap = new Dictionary<int, MatchResult>();
        matchMap.Add(1,_resultGroup.First(x=> x.Period == (13)));
        matchMap.Add(2,_resultGroup.First(x=> x.Period == (9)));
        matchMap.Add(3,_resultGroup.First(x=> x.Period == (8)));
        matchMap.Add(4,_resultGroup.First(x=> x.Period == (7)));
        matchMap.Add(5,_resultGroup.First(x=> x.Period == (6)));
        matchMap.Add(6,_resultGroup.First(x=> x.Period == (5)));

        var weight = new List<IntRange>();
        weight.Add(new IntRange(0, 1, 65f));
        weight.Add(new IntRange(1, 2, 9f));
        weight.Add(new IntRange(2, 3, 8f));
        weight.Add(new IntRange(3, 4, 7f));
        weight.Add(new IntRange(4, 5, 6f));
        weight.Add(new IntRange(5, 6, 5f));


        var result = new int[100];
        for (int i = 0; i < 100; i++)
        {  
            var value = RandomRange.Range(weight.ToArray());
            // var value =  Random.Range(1,7);
            Debug.Log($"[{i}]" + matchMap[value].Id);

            result[i] = value;
            
            var pIndex = matchMap[1].GetPeriodIndex(i);
            var pMax = matchMap[1].MaxRange[pIndex];
            var pMin = matchMap[1].MinRange[pIndex];
            
            var pCount = result.Skip(pMin).Take(matchMap[1].FrequencyTable[pIndex]).Count();
            if (pMax - pCount == i)
            {
                weight.Clear();
                weight.Add(new IntRange(0, 1, 100f));
            }
            else
            {
                weight.Clear();
                weight.Add(new IntRange(0, 1, 65f));
                weight.Add(new IntRange(1, 2, 9f));
                weight.Add(new IntRange(2, 3, 8f));
                weight.Add(new IntRange(3, 4, 7f));
                weight.Add(new IntRange(4, 5, 6f));
                weight.Add(new IntRange(5, 6, 5f));
            }

        
            
        }
        
    }

    public void NewAlogrithm()
    {
        MatchResult[] results = new MatchResult[100];
        List<MatchResult> active = _resultGroup.ToList();
        Dictionary<int,MatchResult> passive = new Dictionary<int, MatchResult>();

        for (int i = 0; i < 100; i++)
        {  
            foreach (var a in active.ToArray())
            {
                var p = a.GetPeriodIndex(i);
                var maxRange = a.Period != 13 ? a.GetRange(p+1)-1 :a.GetRange(p+1) - active.Count(x=> x.Period == 13)-1 ;
                if(maxRange == i)
                {    
                    active.Add(results[i]);
                    results[i] = a;
                    passive[i] = a;
                }
            }
            
            // results[i] = active.ElementAt(Random.Range(0,active.Count));
            // passive.Add(i,results[i]);
            // active.Remove(results[i]);
            
            // foreach (var p in passive)
            // {
            //     if(p.Value.GetPeriodIndex(p.Key) < p.Value.GetPeriodIndex(i))
            //     {
            //         active.Add(p.Value);
            //     }
            // }   
        }  
        var msg = string.Join("\n",results.Select((x,i) => $"[{i}]" + "[" + x.Id.ToString() + "]" + "--" + $"%{x.Period}"));
        Debug.Log(msg);
        System.IO.File.WriteAllText("Results.txt", msg);
    }

    public void CreateResult()
    {
        var _indexList = Enumerable.Range(0, 30).ToList();
        string[] r = Enumerable.Repeat("Empty", 30).ToArray();
        
        foreach (var result in _resultGroup)
        {
            var msg = "[INFO]" + string.Join(",", result.Match) + "Index";
            for (int i = 0; i < result.Period; i++)
            {

                IEnumerable<int> remainingIndex = null;
                var index = 0;
                try
                {
                    remainingIndex = _indexList.Where(x => x >= result.GetRange(i) && x < result.GetRange(i + 1));
                    var randomIndex = Random.Range(0, remainingIndex.Count());
                    if (remainingIndex.Count() == 0)
                    {
                        msg += $"[Error] No Item Found In Range [{result.GetRange(i)} {result.GetRange(i + 1)}]";
                    }
                    else
                    {
                        index = remainingIndex.ElementAt(randomIndex);
                        _indexList.Remove(index);
                        _result.Add(index, result);
                        r[i] = result.Id;
                        msg += "," + index;
                    }

                }
                catch (System.Exception e)
                {  
                    _result.Add(index, result);
                    // Debug.Log($"[Error]{e.Message}");
                }

            }
            Debug.Log(msg);
        }
        _result = _result.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

        // Debug.Log(string.Join("\n", _result.Select(x => "[" + x.Key.ToString() + "]" + "--" + string.Join(",", x.Value.Match))));
        // var files = string.Join("\n", _result.Select(x => "[" + x.Key.ToString() + "]" + "--" + string.Join(",", x.Value.Match) +" Percentage %:" + (x.Value.Period) ));
        // files += "\n Missing" + string.Join(",", _indexList);
        // System.IO.File.WriteAllText("Results.txt", files);
        //System.IO.File.WriteAllText("Results.txt", string.Join(",",r));
        //Debug.Log("Missing" + string.Join(",", _indexList));



        //var results = _result.Select(x => x.Value).ToArray();
        //Test(_result);
    }

    public void Test(Dictionary <int,MatchResult> result)
    {
        foreach (var group in _resultGroup)
        {
            var groupResults = result.Where(x=> string.Equals(x.Value.Id,group.Id)).Select(x=>x.Key).ToArray();
            if(group.Period  != groupResults.Length)
            {
                Debug.Assert(false,$"More or none item in Period %{group.Period},[{group.Id}]");
                continue;
            }

            for (int i = 0; i < group.Period; i++)
            {
                var rangeMin = group.GetRange(i);
                var rangeMax = group.GetRange(i + 1);
//                Debug.Log($"Value:{groupResults[i]} Period%{group.Period},[{group.Id}] Range[{rangeMin},{rangeMax}]");
                var assert = groupResults[i] >= rangeMin && groupResults[i] < rangeMax;      
                Debug.Assert(assert,$"More or none item in Period %{group.Period},[{group.Id}] Range[{rangeMin},{rangeMax}]");
               
            }
        }
    }

    private void CreateResultGroups()
    {

        _resultGroup.Add(new MatchResult(new int[] { 8, 8, 8, 8, 7, 8, 7, 7, 8, 8, 8, 8, 7 })
        {
            Match = new string[] { "Bonus", "A", "JackPot" },
        });
        _resultGroup.Add(new MatchResult(new int[] { 8, 8, 8, 8, 7, 8, 7, 7, 8, 8, 8, 8, 7 })
        {
            Match = new string[] { "Wild", "Bonus", "A" },
        });

        _resultGroup.Add(new MatchResult(new int[] { 8, 8, 8, 8, 7, 8, 7, 7, 8, 8, 8, 8, 7 })
        {
            Match = new string[] { "JackPot", "JackPot", "A" },
        });
        _resultGroup.Add(new MatchResult(new int[] { 8, 8, 8, 8, 7, 8, 7, 7, 8, 8, 8, 8, 7 })
        {
            Match = new string[] { "Wild", "Wild", "Seven" },

        });
        _resultGroup.Add(new MatchResult(new int[] { 8, 8, 8, 8, 7, 8, 7, 7, 8, 8, 8, 8, 7 })
        {
            Match = new string[] { "A", "Wild", "Bonus" },
        });

        _resultGroup.Add(new MatchResult(new int[] { 10, 11, 11, 11, 11, 11, 11, 12, 12})
        {
            Match = new string[] { "A", "A", "A" },
        });

        _resultGroup.Add(new MatchResult(new int[] { 13, 12, 13, 12, 13, 12, 12, 13 })
        {
            Match = new string[] { "Bonus", "Bonus", "Bonus" },
        });
        _resultGroup.Add(new MatchResult(new int[] { 14, 14, 15, 15, 14, 14, 14 })
        {
            Match = new string[] { "Seven", "Seven", "Seven" },
        });
        _resultGroup.Add(new MatchResult(new int[] { 17, 16, 17, 16, 17, 17 })
        {
            Match = new string[] { "Wild", "Wild", "Wild" },
        });
        _resultGroup.Add(new MatchResult(new int[] { 20, 20, 20, 20, 20 })
        {
            Match = new string[] { "JackPot", "JackPot", "JackPot" },
        });

        _resultGroup = _resultGroup.OrderBy(x=> x.Period).ToList();

        foreach (var result in _resultGroup)
        {
            Debug.Assert(result.Period == result.FrequencyTable.Count(), $"{result.Id}Period Not equal with frequency table");
            Debug.Assert(result.FrequencyTable.Sum() == 100, $"{result.Id} Period sum not equal 100");

            result.PrintBoundires();
        }
    }

    public struct IntRange
    {
 
        public int Min;
        public int Max;
        public float Weight;

        public IntRange(int min, int max, float weight)
        {
            Min = min;
            Max = max;
            Weight = weight;
        }
    }
 
    public struct FloatRange
    {
        public float Min;
        public float Max;
        public float Weight;
    }
 
    public static class RandomRange
    {
 
        public static int Range(params IntRange[] ranges)
        {
            if (ranges.Length == 0) throw new System.ArgumentException("At least one range must be included.");
            if (ranges.Length == 1) return Random.Range(ranges[0].Max, ranges[0].Min);
 
            float total = 0f;
            for (int i = 0; i < ranges.Length; i++) total += ranges[i].Weight;
 
            float r = Random.value;
            float s = 0f;
 
            int cnt = ranges.Length - 1;
            for (int i = 0; i < cnt; i++)
            {
                s += ranges[i].Weight / total;
                if (s >= r)
                {
                    return Random.Range(ranges[i].Max, ranges[i].Min);
                }
            }
 
            return Random.Range(ranges[cnt].Max, ranges[cnt].Min);
        } 
    }

}
