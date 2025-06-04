using System.Text.RegularExpressions;

namespace HackAssembler;

public static class Validator
{
	public static bool IsCorrectSymbol(string symbol)
	{
		return Regex.IsMatch(symbol, @"^[a-zA-Z._$:][a-zA-Z0-9._$:]*$");
	}
	public static bool IsCorrectConstant(string constant)
	{
		if (ushort.TryParse(constant, out ushort number))
		{
			return 0 <= number && 32767 >= number;
		}
		return false;
	}
}