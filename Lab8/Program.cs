using System;
using System.IO;
using System.Text.Json;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Lab8
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				var settings = new XmlReaderSettings();
				settings.Schemas.Add("", "gun_schema.xsd");
				settings.ValidationType = ValidationType.Schema;

				using (var reader = XmlReader.Create("gun.xml", settings))
				{
					var document = new XmlDocument();
					document.Load(reader);

					var eventHandler = new ValidationEventHandler(ValidationEventHandler);

					document.Validate(eventHandler);
				}

				var formatter = new XmlSerializer(typeof(Gun));
				using (var xmlFs = new FileStream("gun.xml", FileMode.Open))
				{
					var gun = (Gun)formatter.Deserialize(xmlFs);

					using (var jsonFs = File.Create("gun.json"))
					{
						JsonSerializer.SerializeAsync(jsonFs, gun);
					}
				}

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			Console.WriteLine("Done!");
		}

		static void ValidationEventHandler(object sender, ValidationEventArgs e)
		{
			switch (e.Severity)
			{
				case XmlSeverityType.Error:
					Console.WriteLine("Error: {0}", e.Message);
					break;
				case XmlSeverityType.Warning:
					Console.WriteLine("Warning {0}", e.Message);
					break;
			}
		}
	}
}
