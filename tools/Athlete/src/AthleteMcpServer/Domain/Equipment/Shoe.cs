using System;
using System.Collections.Generic;
using System.Text;

namespace AthleteMcpServer.Domain.Equipment
{

    public sealed class Shoe
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public Guid AthleteId { get; private set; }

        public string Name { get; private set; } = string.Empty;

        public string? Brand { get; private set; }

        public string? Model { get; private set; }

        public DateOnly? FirstUsedOn { get; private set; }

        public decimal RetireAfterKm { get; private set; } = 650;

        public decimal DistanceLoggedKm { get; private set; }

        public bool IsRetired { get; private set; }

        private Shoe()
        {
        }

        public Shoe(
            Guid athleteId,
            string name,
            string? brand = null,
            string? model = null,
            DateOnly? firstUsedOn = null,
            decimal retireAfterKm = 650)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            if (retireAfterKm <= 0)
            {
                throw new InvalidOperationException("Retirement distance must be greater than zero.");
            }

            AthleteId = athleteId;
            Name = name;
            Brand = brand;
            Model = model;
            FirstUsedOn = firstUsedOn;
            RetireAfterKm = retireAfterKm;
        }

        public void AddMileage(decimal distanceKm)
        {
            if (distanceKm <= 0)
            {
                throw new InvalidOperationException("Distance must be greater than zero.");
            }

            DistanceLoggedKm += distanceKm;
        }

        public void Retire()
        {
            IsRetired = true;
        }
    }
}
