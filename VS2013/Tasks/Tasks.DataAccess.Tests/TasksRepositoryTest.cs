﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Tasks.DataAccess.Tasks;
using FluentAssertions;
using Moq;
using System.Linq;

namespace Tasks.DataAccess.Tests
{
    [TestClass]
    public class TasksRepositoryTest
    {
        private DomainModel.User _creatorUser;
        private List<DomainModel.Task> _tasks;

        [TestInitialize]
        public void InitializeTest()
        {
            _creatorUser = new DomainModel.User()
            { 
                UserID = 1,
                Username = "federico.orlandini"
            };

            _tasks = new List<DomainModel.Task>()
            {
                new DomainModel.Task { 
                    ID = 1,
                    Created = new DateTime(2014, 12, 25), 
                    Creator = _creatorUser, 
                    Description = "This is the first task for our tests - Not Started", 
                    EstimatedHours = 1, 
                    Status = DomainModel.TaskStatus.NotStarted, 
                    Title = "First task" 
                },
                new DomainModel.Task { 
                    ID = 2,
                    Created = new DateTime(2014, 12, 27), 
                    Creator = _creatorUser, 
                    Description = "This is the second task for our tests - In Progress", 
                    EstimatedHours = 1, 
                    Status = DomainModel.TaskStatus.InProgress, 
                    Title = "Second task" 
                },
                new DomainModel.Task { 
                    ID = 3,
                    Created = new DateTime(2015, 1, 1), 
                    Creator = _creatorUser, 
                    Description = "This is the third task for our tests - Not Started", 
                    EstimatedHours = 1, 
                    Status = DomainModel.TaskStatus.NotStarted, 
                    Title = "Third task" 
                }

            };

        }
        
        [TestMethod]
        public void GetAll_SimpleCall_ShouldReturnAllTheTasks()
        {
            // Arrange
            var repository = new TasksRepository(_tasks);

            // Act
            var allTasks = repository.GetAll();

            // Assert
            allTasks.ShouldAllBeEquivalentTo(_tasks, "because the GetAll() method should return all the tasks in the repository's collection");
        }

        [TestMethod]
        public void GetAll_FilteringByStatus_ShouldReturnTasksWithTheCorrectStatus()
        {
            // Arrange
            var statusUnderTest = DomainModel.TaskStatus.InProgress;
            int[] inProgressTasksID = new int[] { 2 };
            var repository = new TasksRepository(_tasks);

            // Act
            var filteredTasks = repository.GetAll(statusUnderTest);

            // Assert
            filteredTasks.Should().HaveCount(inProgressTasksID.Length, "because the result should contains only one task");
            filteredTasks.Should().OnlyContain(t => t.Status == statusUnderTest, "because the result should contains only tasks with the correct status");
        }

        [TestMethod]
        public void Add_SingleEntity_ShouldAddTheEntityInTheCollection()
        {
            // Arrange
            var taskToAdd = new DomainModel.Task() { 
                Title = "A task to add"
            };
            
            // Act
            var repository = new TasksRepository(_tasks);
            repository.Add(taskToAdd);

            // Assert
            _tasks.Should().Contain(taskToAdd, "because the new entity should have been added to the repository");
        }

        [TestMethod]
        public void Add_SingleEntity_ShouldAssignTheIdToTheNewEntity()
        {
            // Arrange
            var taskToAdd = new DomainModel.Task()
            {
                Title = "A task to add"
            };

            // Act
            var repository = new TasksRepository(_tasks);
            repository.Add(taskToAdd);

            // Assert
            taskToAdd.Should().Match<DomainModel.Task>(t => t.ID != 0, "because the repository should assign a valid ID to the new entity");
        }

        [TestMethod]
        public void Delete_EntityAlreadyInTheRepository_ShouldRemoveTheEntityFromTheRepository()
        {
            // Arrange
            var taskToDelete = new DomainModel.Task() { 
                ID = _tasks[0].ID 
            };

            // Act
            var repository = new TasksRepository(_tasks);
            repository.Delete(taskToDelete);

            // Assert
            _tasks.Should().NotContain(taskToDelete, "because the entity should be removed from the collection");
        }

        [TestMethod]
        public void Delete_EntityNotInTheRepository_ShouldLeaveTheCollectionUntouched()
        {
            // Arrange
            var originalCollection = _tasks.ToArray();
            var taskToDelete = new DomainModel.Task()
            {
                ID = 1000
            };

            // Act
            var repository = new TasksRepository(_tasks);
            repository.Delete(taskToDelete);

            // Assert
            _tasks.ShouldAllBeEquivalentTo(originalCollection, "because the collection should not has been changed");
        }

        [TestMethod]
        public void Update_EntityAlreadyInTheRepository_ShouldUpdateTheEntityInTheCollection()
        {
            // Arrange
            var taskToUpdate = new DomainModel.Task() { 
                ID = _tasks[0].ID,
                Created = _tasks[0].Created,
                Creator = _tasks[0].Creator, 
                Description = "This is a new description", 
                Title = "This is a new title", 
                Status = DomainModel.TaskStatus.InTest, 
                EstimatedHours = 100
            }; 

            // Act
            var repository = new TasksRepository(_tasks);
            repository.Update(taskToUpdate);

            // Assert
            var entityInCollection = _tasks.SingleOrDefault(t => t.ID == taskToUpdate.ID);
            entityInCollection.Should().NotBeNull("because the entity should be in the collection").And.ShouldBeEquivalentTo(taskToUpdate, opt => opt.ExcludingMissingProperties(), "because the update should update all the properties in the entity");
        }

        [TestMethod]
        public void Update_EntityNotInTheRepository_ShouldLeaveTheCollectionUntouched()
        {
            // Arrange
            var originalCollection = _tasks.ToArray();
            var taskToUpdate = new DomainModel.Task()
            {
                ID = 1000,
                Created = DateTime.Now,
                Creator = _creatorUser,
                Description = "This is a description",
                Title = "This is a title",
                Status = DomainModel.TaskStatus.InTest,
                EstimatedHours = 100
            }; 

            // Act
            var repository = new TasksRepository(_tasks);
            repository.Update(taskToUpdate);

            // Assert
            originalCollection.ShouldAllBeEquivalentTo(_tasks, "the tasks collection in the repository should not be changed");
        }

        [TestMethod]
        public void FindById_TheIdIsRelatedToAnEntityAlreadyInTheRepository_ShouldReturnTheCorrectEntity()
        {
            // Arrange
            var taskToSearch = _tasks[0];

            // Act
            var repository = new TasksRepository(_tasks);
            var taskFound = repository.FindById(taskToSearch.ID);

            // Assert
            taskFound.ShouldBeEquivalentTo(taskToSearch, "because the repository should return the correct task");
        }

        [TestMethod]
        public void FindById_TheIdIsRelatedToAnEntityNotInTheRepository_ShouldReturnNull()
        {
            // Arrange
            var taskToSearchId = 1000;

            // Act
            var repository = new TasksRepository(_tasks);
            var taskFound = repository.FindById(taskToSearchId);

            // Assert
            taskFound.Should().BeNull("because the repository should return null if the searched entity is not present");
        }
    }
}