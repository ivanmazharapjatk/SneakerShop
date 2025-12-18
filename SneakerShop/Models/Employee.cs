using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SneakerShop.Models
{
    public class Employee : ICustomerSupportAgent, ILogisticsCoordinator
    {
        // CLASS EXTENT FOR EMPLOYEE
        private static readonly List<Employee> _extent = new();
        public static IReadOnlyList<Employee> Extent => _extent.AsReadOnly();
        private readonly List<Employee> _subordinates = new();
        public IReadOnlyList<Employee> Subordinates => _subordinates.AsReadOnly();
        public Employee? Supervisor { get; private set; }
        
        public string? ContactNumber { get; set; }
        public void RespondToReview(Review review)
        {
            //TODO: method's logic
        }

        public List<Supply>? AssignedSupplies { get; set; }
        public void AskForSupply(Supplier supplier)
        {
            //TODO: method's logic
        }

        private const string ExtentFilePath = "EmployeeExtent.json";

        public static void ClearExtent()
        {
            foreach (var employee in _extent)
            {
                employee._subordinates.Clear();
                employee.Supervisor = null;
            }
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
        
        public decimal GetSalaryNow()
        {
            return BaseSalary + (Bonus ?? 0m);
        }

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
        
        public Employee(
            string name,
            string surname,
            string position,
            int clearanceLevel,
            DateTime hireDate,
            string contactNumber
        ) : this(name, surname, position, clearanceLevel, hireDate)
        {
            if (string.IsNullOrWhiteSpace(contactNumber))
                throw new ArgumentException("Customer support agent must have a contact number.");

            ContactNumber = contactNumber;
        }
        
        public Employee(
            string name,
            string surname,
            string position,
            int clearanceLevel,
            DateTime hireDate,
            List<Supply> assignedSupplies
        ) : this(name, surname, position, clearanceLevel, hireDate)
        {
            AssignedSupplies = assignedSupplies ?? throw new ArgumentNullException(
                nameof(assignedSupplies),
                "Logistics coordinator must have an assigned supplies list."
            );
        }
        
        public void AssignSupervisor(Employee supervisor)
        {
            if (supervisor == null) throw new ArgumentNullException(nameof(supervisor));
            if (ReferenceEquals(this, supervisor))
                throw new InvalidOperationException("Employee cannot supervise themselves.");
            if (CreatesCycle(supervisor))
                throw new InvalidOperationException("Cannot assign supervisor that would create a supervision cycle.");

            if (Supervisor == supervisor) return;

            Supervisor?.RemoveSubordinateInternal(this);
            supervisor.AddSubordinateInternal(this);
        }

        public void RemoveSupervisor()
        {
            Supervisor?.RemoveSubordinateInternal(this);
        }

        public void RemoveSubordinate(Employee subordinate)
        {
            if (subordinate == null) throw new ArgumentNullException(nameof(subordinate));
            if (!_subordinates.Contains(subordinate))
                throw new InvalidOperationException("Employee is not supervising this subordinate.");

            RemoveSubordinateInternal(subordinate);
        }

        private void AddSubordinateInternal(Employee subordinate)
        {
            if (_subordinates.Contains(subordinate)) return;

            _subordinates.Add(subordinate);
            subordinate.Supervisor = this;
        }

        private void RemoveSubordinateInternal(Employee subordinate)
        {
            if (_subordinates.Remove(subordinate))
            {
                if (subordinate.Supervisor == this)
                {
                    subordinate.Supervisor = null;
                }
            }
        }

        private bool CreatesCycle(Employee potentialSupervisor)
        {
            var current = potentialSupervisor;
            while (current != null)
            {
                if (ReferenceEquals(current, this))
                {
                    return true;
                }
                current = current.Supervisor;
            }

            return false;
        }
        
    }
}
