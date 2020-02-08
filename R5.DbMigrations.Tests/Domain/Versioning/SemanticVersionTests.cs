using R5.DbMigrations.Domain.Versioning;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace R5.DbMigrations.Tests.Domain.Versioning
{
	public class SemanticVersionTests
	{
		public class Constructor
		{
			[Fact]
			public void NegativeMajor_Throws()
			{
				Assert.Throws<ArgumentOutOfRangeException>
					(() => new SemanticVersion(-1, 0, 0));
			}

			[Fact]
			public void NegativeMinor_Throws()
			{
				Assert.Throws<ArgumentOutOfRangeException>
					(() => new SemanticVersion(0, -1, 0));
			}

			[Fact]
			public void NegativePatch_Throws()
			{
				Assert.Throws<ArgumentOutOfRangeException>
					(() => new SemanticVersion(0, 0, -1));
			}
		}

		public class ImplicitConversionFromString
		{
			[Theory]
			[InlineData("")]
			[InlineData("1")]
			[InlineData("1.1")]
			[InlineData("1.1.1.1")]
			[InlineData("1.1.1.1.1")]
			public void TokenSplitCount_OtherThanThree_Throws(string version)
			{
				Assert.Throws<InvalidOperationException>(() =>
				{
					SemanticVersion sv = version;
				});
			}
		}

		public class CompareTo
		{
			[Fact]
			public void OtherNull_ThisGreater()
			{
				SemanticVersion v = SemanticVersion.MinValue;
				int actual = v.CompareTo(null);
				Assert.Equal(1, actual);
			}

			[Theory]
			[InlineData("1.0.0", "0.0.0", 1)]
			[InlineData("0.0.0", "1.0.0", -1)]
			[InlineData("1.1.0", "1.0.0", 1)]
			[InlineData("1.0.0", "1.1.0", -1)]
			[InlineData("1.1.1", "1.1.0", 1)]
			[InlineData("1.1.0", "1.1.1", -1)]
			[InlineData("1.1.1", "1.1.1", 0)]
			public void ReturnsExpectedValue(string version, string otherVersion, int expected)
			{
				SemanticVersion v = version;
				SemanticVersion other = otherVersion;
				int actual = v.CompareTo(other);
				Assert.Equal(expected, actual);
			}
		}

		public class IEquatable
		{
			[Fact]
			public void OtherNull_False()
			{
				SemanticVersion v = SemanticVersion.MinValue;
				SemanticVersion other = null;
				Assert.False(v.Equals(other));
			}

			[Theory]
			[InlineData("1.0.0", "0.0.0", false)]
			[InlineData("0.0.0", "1.0.0", false)]
			[InlineData("1.1.0", "1.0.0", false)]
			[InlineData("1.0.0", "1.1.0", false)]
			[InlineData("1.1.1", "1.1.0", false)]
			[InlineData("1.1.0", "1.1.1", false)]
			[InlineData("1.1.1", "1.1.1", true)]
			public void ReturnsExpectedValue(string version, string otherVersion, bool expected)
			{
				SemanticVersion v = version;
				SemanticVersion other = otherVersion;
				bool actual = v.Equals(other);
				Assert.Equal(expected, actual);
			}
		}

		public class EqualsOverride
		{
			[Fact]
			public void Other_IsDifferentType_False()
			{
				SemanticVersion v = SemanticVersion.MinValue;
				YearQuarterVersion other = YearQuarterVersion.MinValue;
				Assert.False(v.Equals(other));
			}

			[Fact]
			public void Other_IsNull_False()
			{
				SemanticVersion v = SemanticVersion.MinValue;
				object other = null;
				Assert.False(v.Equals(other));
			}

			[Theory]
			[InlineData("1.0.0", "0.0.0", false)]
			[InlineData("0.0.0", "1.0.0", false)]
			[InlineData("1.1.0", "1.0.0", false)]
			[InlineData("1.0.0", "1.1.0", false)]
			[InlineData("1.1.1", "1.1.0", false)]
			[InlineData("1.1.0", "1.1.1", false)]
			[InlineData("1.1.1", "1.1.1", true)]
			public void ReturnsExpectedValue(string version, string otherVersion, bool expected)
			{
				SemanticVersion v = version;
				SemanticVersion other = otherVersion;
				bool actual = v.Equals((object)other);
				Assert.Equal(expected, actual);
			}
		}

		public class GetHashCodeOverride
		{
			[Theory]
			[InlineData("1.0.0", "1.0.0", true)]
			[InlineData("1.1.0", "1.1.0", true)]
			[InlineData("1.1.1", "1.1.1", true)]
			[InlineData("0.0.0", "1.0.0", false)]
			[InlineData("0.0.0", "1.1.0", false)]
			[InlineData("0.0.0", "1.1.1", false)]
			[InlineData("1.1.0", "1.0.0", false)]
			[InlineData("1.1.0", "1.0.1", false)]
			[InlineData("1.1.1", "1.0.0", false)]
			[InlineData("1.1.1", "1.1.0", false)]
			[InlineData("1.1.1", "1.1.2", false)]
			public void ReturnsExpectedValue(string version, string otherVersion, bool hashesAreEqual)
			{
				SemanticVersion v = version;
				SemanticVersion other = otherVersion;
				bool actual = v.GetHashCode() == other.GetHashCode();
				Assert.Equal(hashesAreEqual, actual);
			}
		}
	}
}
