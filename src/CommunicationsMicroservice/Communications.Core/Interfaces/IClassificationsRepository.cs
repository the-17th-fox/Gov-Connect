﻿using Communications.Core.Models;

namespace Communications.Core.Interfaces;

public interface IClassificationsRepository : IGenericRepository<Classification>
{
    public Task<Classification?> GetByNameAsync(string name);
}