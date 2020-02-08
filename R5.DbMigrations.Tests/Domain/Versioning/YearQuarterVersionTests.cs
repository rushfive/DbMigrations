using R5.DbMigrations.Domain.Versioning;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace R5.DbMigrations.Tests.Domain.Versioning
{
	public class YearQuarterVersionTests
	{
		public class Year
		{
			[Fact]
			public void Under_2020_Throws()
			{
				Assert.Throws<ArgumentOutOfRangeException>
					(() => new YearQuarterVersion(2019, 1, 0));
			}

			[Fact]
			public void Over_FourDigits_Throws()
			{
				Assert.Throws<ArgumentOutOfRangeException>
					(() => new YearQuarterVersion(10_000, 1, 0));
			}
		}

		public class Month
		{
			[Fact]
			public void Under_1_Throws()
			{
				Assert.Throws<ArgumentOutOfRangeException>
					(() => new YearQuarterVersion(2020, 0, 0));
			}

			[Fact]
			public void Over_4_Throws()
			{
				Assert.Throws<ArgumentOutOfRangeException>
					(() => new YearQuarterVersion(2020, 5, 0));
			}
		}

		public class Version
		{
			[Fact]
			public void Negative_Throws()
			{
				Assert.Throws<ArgumentOutOfRangeException>
					(() => new YearQuarterVersion(2020, 4, -1));
			}
		}

		public class ImplicitConversionFromString
		{
			[Theory]
			[InlineData("")]
			[InlineData("2020")]
			[InlineData("2020.1")]
			[InlineData("2020.1.1.1")]
			[InlineData("2020.1.1.1.1")]
			public void TokenSplitCount_OtherThanThree_Throws(string version)
			{
				Assert.Throws<InvalidOperationException>(() =>
				{
					YearQuarterVersion sv = version;
				});
			}
		}

		public class CompareTo
		{
			[Fact]
			public void OtherNull_ThisGreater()
			{
				YearQuarterVersion v = YearQuarterVersion.MinValue;
				int actual = v.CompareTo(null);
				Assert.Equal(1, actual);
			}

			[Theory]
			[InlineData("2021.1.0", "2020.1.0", 1)]
			[InlineData("2020.1.0", "2021.1.0", -1)]
			[InlineData("2020.2.0", "2020.1.0", 1)]
			[InlineData("2020.1.0", "2020.2.0", -1)]
			[InlineData("2020.1.1", "2020.1.0", 1)]
			[InlineData("2020.1.0", "2020.1.1", -1)]
			[InlineData("2020.1.0", "2020.1.0", 0)]
			public void ReturnsExpectedValue(string version, string otherVersion, int expected)
			{
				YearQuarterVersion v = version;
				YearQuarterVersion other = otherVersion;
				int actual = v.CompareTo(other);
				Assert.Equal(expected, actual);
			}
		}

		public class IEquatable
		{
			[Fact]
			public void OtherNull_False()
			{
				YearQuarterVersion v = YearQuarterVersion.MinValue;
				YearQuarterVersion other = null;
				Assert.False(v.Equals(other));
			}

			[Theory]
			[InlineData("2021.1.0", "2020.1.0", false)]
			[InlineData("2020.1.0", "2021.1.0", false)]
			[InlineData("2020.2.0", "2020.1.0", false)]
			[InlineData("2020.1.0", "2020.2.0", false)]
			[InlineData("2020.1.1", "2020.1.0", false)]
			[InlineData("2020.1.0", "2020.1.1", false)]
			[InlineData("2020.1.0", "2020.1.0", true)]
			public void ReturnsExpectedValue(string version, string otherVersion, bool expected)
			{
				YearQuarterVersion v = version;
				YearQuarterVersion other = otherVersion;
				bool actual = v.Equals(other);
				Assert.Equal(expected, actual);
			}
		}

		public class EqualsOverride
		{
			[Fact]
			public void Other_IsDifferentType_False()
			{
				var v = YearQuarterVersion.MinValue;
				var other = SemanticVersion.MinValue;
				Assert.False(v.Equals(other));
			}

			[Fact]
			public void Other_IsNull_False()
			{
				var v = YearQuarterVersion.MinValue;
				object other = null;
				Assert.False(v.Equals(other));
			}

			[Theory]
			[InlineData("2021.1.0", "2020.1.0", false)]
			[InlineData("2020.1.0", "2021.1.0", false)]
			[InlineData("2020.2.0", "2020.1.0", false)]
			[InlineData("2020.1.0", "2020.2.0", false)]
			[InlineData("2020.1.1", "2020.1.0", false)]
			[InlineData("2020.1.0", "2020.1.1", false)]
			[InlineData("2020.1.0", "2020.1.0", true)]
			public void ReturnsExpectedValue(string version, string otherVersion, bool expected)
			{
				YearQuarterVersion v = version;
				YearQuarterVersion other = otherVersion;
				bool actual = v.Equals((object)other);
				Assert.Equal(expected, actual);
			}
		}

		public class GetHashCodeOverride
		{
			[Theory]
			[InlineData("2020.1.0", "2020.1.0", true)]
			[InlineData("2020.2.0", "2020.2.0", true)]
			[InlineData("2020.1.1", "2020.1.1", true)]
			[InlineData("2020.1.0", "2021.1.0", false)]
			[InlineData("2020.1.0", "2021.2.0", false)]
			[InlineData("2020.1.0", "2020.1.1", false)]
			[InlineData("2020.2.0", "2020.1.0", false)]
			[InlineData("2020.2.0", "2020.1.1", false)]
			[InlineData("2020.2.1", "2020.1.0", false)]
			[InlineData("2020.2.1", "2020.2.0", false)]
			[InlineData("2020.2.1", "2020.2.2", false)]
			public void ReturnsExpectedValue(string version, string otherVersion, bool hashesAreEqual)
			{
				YearQuarterVersion v = version;
				YearQuarterVersion other = otherVersion;
				bool actual = v.GetHashCode() == other.GetHashCode();
				Assert.Equal(hashesAreEqual, actual);
			}
		}
	}
}
