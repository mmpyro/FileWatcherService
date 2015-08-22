﻿using System;

namespace FileNotifier
{
    public class ObserveFileDto
    {
        public ObserveFileDto(string fileDirectoryPath, bool withSubDirectories = true)
        {
            DirectoryPath = fileDirectoryPath;
            WithSubDirectories = withSubDirectories;
            Filter = string.Empty;
        }

        public ObserveFileDto()
        {
            
        }

        public string DirectoryPath { get; set; }
        public string Filter { get; set; }
        public bool WithSubDirectories { get; set; }

        public override int GetHashCode()
        {
            return DirectoryPath.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var dto = obj as ObserveFileDto;
            if (obj != null)
            {
                return dto.DirectoryPath.Equals(DirectoryPath, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }
    }
}