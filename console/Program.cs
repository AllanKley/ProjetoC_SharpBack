﻿using DAO;
using DTO;
using Model;

using (var context = new DAOContext()){
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();
}


