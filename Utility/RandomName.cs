using Godot;
using System;

public static class RandomName
{
	public static string RandomCharacterName()
	{
		string[] names = new string[] {"Prem", "Anđelko", "Miguel", "Marju", "Abdelhak", "Khurshid", "Cezar", "Polya", "Alex", "Katsu", "Sirje", "Jadranka", "Ganesh", "Abubakar", "Yishai", "Polissena", "Augustín", "Aella", "Hōkūlani", "Aradhana", "Ekene", "Tryphaina", "Oakleigh", "Europa", "Yao", "Kristofor", "Gabija", "Porfirio", "Alparslan", "Uhuru", "Muhammadu", "Farhad", "Radoslava", "Are", "Adel", "Tadhg", "Touthmosis"};
		Random random = new Random();
		return names[random.Next(names.Length)];
	}

	public static string RandomGender()
	{
		string[] genders = new string[] {"Male", "Female"};
		Random random = new Random();
		return genders[random.Next(genders.Length)];
	}

	public static string RandomRune()
	{
		// Null Water Fire Electric Leaf Gear
		string[] genders = new string[] {"Fire", "Water", "Gear", "Leaf", "Electric"};
		Random random = new Random();
		return genders[random.Next(genders.Length)];
	}

	public static string RandomSpecies()
	{
		string[] species = new string[] {"Human", "Kobold", "Avian", "Avali", "Robot"};
		Random random = new Random();
		return species[random.Next(species.Length)];
	}
}
