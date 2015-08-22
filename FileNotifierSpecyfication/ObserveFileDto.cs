using System;

namespace FileNotifierSpecyfication
{
    public class ObserveFileDto
    {
        public string Path { get; set; }
        public string Filter { get; set; }
        public bool WithSubDirectories { get; set; }

        public override int GetHashCode()
        {
            return Path.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var dto = obj as ObserveFileDto;
            if (obj != null)
            {
                return dto.Path.Equals(Path, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }
    }
}