using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SneakerShop.Models
{
    public class Employee
    {
        // CLASS EXTENT FOR EMPLOYEE
        private static readonly List<Employee> _extent = new();
        public static IReadOnlyList<Employee> Extent => _extent.AsReadOnly();
        
        private const string ExtentFilePath = "EmployeeExtent.json";

        public static void ClearExtent()
        {
            _extent.Clear();
        }
        
        // Saves the class extent of Employee to a JSON file
        public static void SaveExtent()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(_extent, options);
            File.WriteAllText(ExtentFilePath, json);
        }
        
        // Loads the class extent of Employee from a JSON file
        public static void LoadExtent()
        {
            if (!File.Exists(ExtentFilePath))
            {
                return;
            }

            ClearExtent();

            var json = File.ReadAllText(ExtentFilePath);

            var _ = JsonSerializer.Deserialize<List<Employee>>(json);
        }

        private string _name;
        private string _surname;
        private string _position;
        private int _clearanceLevel;
        private DateTime _hireDate;

        public static decimal BaseSalary { get; set; } = 2500m;

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Name cannot be empty.");
                _name = value;
            }
        }

        public string Surname
        {
            get => _surname;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Surname cannot be empty.");
                _surname = value;
            }
        }

        public string Position
        {
            get => _position;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Position cannot be empty.");
                _position = value;
            }
        }

        public DateTime HireDate
        {
            get => _hireDate;
            set
            {
                if (value > DateTime.Now)
                    throw new ArgumentException("Hire date cannot be in the future.");
                _hireDate = value;
            }
        }

        public decimal? Bonus { get; set; } = 0m;

        public decimal SalaryNow => BaseSalary + (Bonus ?? 0m);

        public int ClearanceLevel
        {
            get => _clearanceLevel;
            set
            {
                if (value < 1 || value > 3)
                    throw new ArgumentOutOfRangeException(
                        nameof(ClearanceLevel),
                        "Clearance level must be between 1 and 3.");
                _clearanceLevel = value;
            }
        }

        public Employee(string name, string surname, string position, int clearanceLevel, DateTime hireDate)
        {
            Name = name;
            Surname = surname;
            Position = position;
            ClearanceLevel = clearanceLevel;
            HireDate = hireDate;

            _extent.Add(this);
        }
    }
}
