<Query Kind="Program">
  <Connection>
    <ID>4bc2b8ad-0542-4ec6-ab67-d8cd8e8b3a06</ID>
    <Persist>true</Persist>
    <Server>localhost</Server>
    <Database>Fintools</Database>
    <ShowServer>true</ShowServer>
  </Connection>
  <NuGetReference>Accord.MachineLearning</NuGetReference>
  <NuGetReference>Accord.Math</NuGetReference>
  <Namespace>Accord.Statistics</Namespace>
  <Namespace>static System.Math</Namespace>
  <Namespace>Accord.Statistics.Distributions.Univariate</Namespace>
</Query>

void Main()
{
	var universe = CompanyStats.Where(cs => cs.MarketCap != null).ToList();
	var totMarket = universe.Sum(cs => (double) cs.MarketCap);
	(var avgFactor, var stdFactor) = stddev(universe.Select(cs => (double) (cs.Avg30Volume ?? 0) ).ToList());
	var index = (from u in universe
		let zscore = normRange( ((double) (u.Avg30Volume ?? 0) - avgFactor) / stdFactor )
		let score = cumNorm(zscore)
		select new { u.Symbol, CapWeight = (double) u.MarketCap / totMarket, FactorScore = score,
			u.MarketCap, u.Avg30Volume
			}).ToList();
	var totFactorWeight = (from i in index select i.CapWeight * i.FactorScore).Sum();
	var adjindex = (from i in index select new { i.Symbol, i.CapWeight, 
	 	i.FactorScore, FactorWeight = (i.FactorScore*i.CapWeight) / totFactorWeight,
		i.MarketCap, i.Avg30Volume
		}).ToList();
		
	adjindex.Where(a => a.FactorWeight > .0001).Dump();
}

// Define other methods and classes here

(double,double) stddev(List<double> vals)
{	
	var valArray = vals.ToArray();
	return (Measures.Mean(valArray), Measures.StandardDeviation(valArray));
}

double normRange(double zscore) {
	if (zscore < -3) return -3;
	if (zscore > 3) return 3;
	return zscore;
}

double cumNorm(double zscore) {
	var dist = new NormalDistribution();
	return dist.DistributionFunction(zscore);
}