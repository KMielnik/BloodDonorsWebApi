﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BloodDonors.Core.Domain;
using BloodDonors.Core.Repositories;
using System.Linq;
using BloodDonors.Infrastructure.Exceptions;

namespace BloodDonors.Infrastructure.Repositories
{
    public class InMemoryDonorRepository : IDonorRepository
    {
        private static readonly ISet<Donor> donors = new HashSet<Donor>();

        public async Task AddAsync(Donor donor)
            => await Task.FromResult(donors.Add(donor));

        public async Task<IEnumerable<Donor>> GetAllAsync()
            => await Task.FromResult(donors);

        public async Task<Donor> GetAsync(string pesel)
            => await Task.FromResult(donors.SingleOrDefault(x => x.Pesel == pesel));

        public async Task DeleteAsync(string pesel)
        {
            var donor = await GetAsync(pesel);
            donors.Remove(donor);
            await Task.CompletedTask;
        }

        public async Task UpdateAsync(Donor donor)
        {
            var oldDonor = donors.SingleOrDefault(x => x.Pesel == donor.Pesel);
            if (oldDonor == null)
                throw new UserNotFoundException("Tried to update non existing donor");
            donors.Remove(oldDonor);
            donors.Add(donor);
            await Task.CompletedTask;
        }
    }
}
