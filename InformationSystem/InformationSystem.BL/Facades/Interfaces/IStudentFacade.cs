﻿using InformationSystem.BL.Models;
using InformationSystem.DAL.Entities;

namespace InformationSystem.BL.Facades.Interfaces;

public interface IStudentFacade : IFacade<StudentEntity, StudentDetailModel, StudentListModel>
{
    
}