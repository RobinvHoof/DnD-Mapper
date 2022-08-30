using System.Collections.Generic;
using System.Drawing;
using System.ComponentModel;
using System.IO;

namespace DTO
{
    public class CampaignDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public UserDTO DM { get; set; }
        public List<UserDTO> Players { get; set; }
        public List<POIDTO> POIs { get; set; }
        public Bitmap Map { get; set; }

        public byte[] MapBinary
        {
            get
            {
                if (Map != null)
                {
                    using (var stream = new MemoryStream())
                    {
                        Map.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                        return stream.ToArray();
                    }
                }
                return new byte[0];
            }
        }
    }
}
