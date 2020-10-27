using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace BTDTextureTool
{
	public class XMLHandler
	{
		public SpriteInformation ImportXML(string filepath)
		{
			try
			{
				SpriteInformation result = null;
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(SpriteInformation));
				using (FileStream stream = File.Open(filepath, FileMode.Open))
				{


					result = (SpriteInformation)xmlSerializer.Deserialize(stream);
				}
				return result;
			}
			catch
			{
				return null;
			}
		}
		public void ExportXML(string filepath, SpriteInformation xml)
		{

				XmlSerializer xmlSerializer = new XmlSerializer(typeof(SpriteInformation));
				using (TextWriter writer = File.CreateText(filepath))
				{
					xmlSerializer.Serialize(writer, xml);
				}



		}

	}
	[XmlRoot(ElementName = "Cell")]
		public class Cell
		{
			[XmlAttribute(AttributeName = "name")]
			public string Name { get; set; }
			[XmlAttribute(AttributeName = "x")]
			public int X { get; set; }
			[XmlAttribute(AttributeName = "y")]
			public int Y { get; set; }
			[XmlAttribute(AttributeName = "w")]
			public int W { get; set; }
			[XmlAttribute(AttributeName = "h")]
			public int H { get; set; }
			[XmlAttribute(AttributeName = "ax")]
			public int Ax { get; set; }
			[XmlAttribute(AttributeName = "ay")]
			public int Ay { get; set; }
			[XmlAttribute(AttributeName = "aw")]
			public int Aw { get; set; }
			[XmlAttribute(AttributeName = "ah")]
			public int Ah { get; set; }
		}

		[XmlRoot(ElementName = "Animation")]
		public class Animation
		{
			[XmlElement(ElementName = "Cell")]
			public List<Cell> Cell { get; set; }
			[XmlAttribute(AttributeName = "name")]
			public string Name { get; set; }
		}

		[XmlRoot(ElementName = "FrameInformation")]
		public class FrameInformation
		{
			[XmlElement(ElementName = "Animation")]
			public List<Animation> Animation { get; set; }
			[XmlElement(ElementName = "Cell")]
			public List<Cell> Cell { get; set; }
			[XmlAttribute(AttributeName = "name")]
			public string Name { get; set; }
			[XmlAttribute(AttributeName = "texw")]
			public int Texw { get; set; }
			[XmlAttribute(AttributeName = "texh")]
			public int Texh { get; set; }
			[XmlAttribute(AttributeName = "type")]
			public string Type { get; set; }
		}

		[XmlRoot(ElementName = "SpriteInformation")]
		public class SpriteInformation
		{
			[XmlElement(ElementName = "FrameInformation")]
			public FrameInformation FrameInformation { get; set; }
		}

	}



