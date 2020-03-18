<Query Kind="Program" />

void Main()
{
	var triangles = new List<List<(double x, double y)>> {
		new List<(double x, double y)> { (0,0),(0,10),(5,10) },
		new List<(double x, double y)> { (0,0),(0,10),(6,10) },
		new List<(double x, double y)> { (0,0),(0,10),(5,10) },
		new List<(double x, double y)> { (1,0),(1,5),(8,0) },
	};

	IsTrue(() => TriangleCompare(triangles[0], triangles[0]) == Intersecting.Enclosed).Show();
	IsTrue(() => TriangleCompare(triangles[0], triangles[1]) == Intersecting.Enclosed).Show();
	IsTrue(() => TriangleCompare(triangles[1], triangles[0]) == Intersecting.Enclosing).Show();
	IsTrue(() => TriangleCompare(triangles[0], triangles[3]) == Intersecting.Overlapping).Show();
	IsTrue(() => TriangleCompare(triangles[3], triangles[0]) == Intersecting.Overlapping).Show();
}

enum Intersecting
{
	Enclosed,
	Enclosing,
	Overlapping,
	Undefined
}

Intersecting TriangleCompare(List<(double x, double y)> t1, List<(double x, double y)> t2)
{
	if (t1.All(t => PointInside(t, t2[0], t2[1], t2[2])))
		return Intersecting.Enclosed;
		
	if (t2.All(t => PointInside(t, t1[0], t1[1], t1[2])))
		return Intersecting.Enclosing;
		
	if (t1.Any(t => PointInside(t, t1[0], t1[1], t1[2])) ||
		t2.Any(t => PointInside(t, t2[0], t2[1], t2[2])))
		return Intersecting.Overlapping;
			
	return Intersecting.Undefined;
}

double sign((double x, double y) p1, (double x, double y) p2, (double x, double y) p3)
{
	return (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
}

bool PointInside((double x, double y) pt, (double x, double y) v1, (double x, double y) v2, (double x, double y) v3)
{
	double d1, d2, d3;
	bool has_neg, has_pos;

	d1 = sign(pt, v1, v2);
	d2 = sign(pt, v2, v3);
	d3 = sign(pt, v3, v1);

	has_neg = (d1 < 0) || (d2 < 0) || (d3 < 0);
	has_pos = (d1 > 0) || (d2 > 0) || (d3 > 0);

	return !(has_neg && has_pos);
}

bool IsTrue(Func<bool> predicate) {
	return predicate();
}

static class Ext
{
	public static void Show(this object o)
	{
		Console.WriteLine(o.ToString());
	}

}