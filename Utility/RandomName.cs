using Godot;
using System;

public static class RandomName
{
	public static string RandomCharacterName()
	{
		string[] names = new string[] {"Prem", "Anđelko", "Miguel", "Marju", "Abdelhak", "Khurshid", "Cezar", "Polya", "Alex", "Katsu", "Sirje", "Jadranka", "Ganesh", "Abubakar", "Yishai", "Polissena", "Augustín", "Aella", "Hōkūlani", "Aradhana", "Ekene", "Tryphaina", "Oakleigh", "Europa", "Agathangelos", "Yao", "Kristofor", "Gabija", "Porfirio", "Alparslan", "Chandrasekhar", "Uhuru", "Muhammadu", "Nabu-Apla-Usur", "Farhad", "Radoslava", "Are", "Adel", "Tadhg", "Touthmosis"};
		Random random = new Random();
		return names[random.Next(names.Length)];
	}

	public static string RandomGender()
	{
		string[] genders = new string[] {"Male", "Female"};
		Random random = new Random();
		return genders[random.Next(genders.Length)];
	}

	public static string RandomSpecies()
	{
		string[] species = new string[] {"Huamn", "Kobold", "Avian", "Avali", "Robot"};
		Random random = new Random();
		return species[random.Next(species.Length)];
	}
}
