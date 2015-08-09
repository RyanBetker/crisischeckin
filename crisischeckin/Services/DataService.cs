﻿using Models;
using Services.Exceptions;
using Services.Interfaces;
using System;
using System.Linq;

namespace Services
{
    public class DataService : IDataService
    {
        // This class does not dispose of the context, because the Ninject library takes care of
        // that for us.

        private readonly CrisisCheckin context;
        private readonly CrisisCheckinMembership membership_context;

        public DataService(CrisisCheckin ctx, CrisisCheckinMembership mctx)
        {
            context = ctx;
            membership_context = mctx;
        }

        public IQueryable<Cluster> Clusters
        {
            get { return context.Clusters; }
        }

        public IQueryable<VolunteerType> VolunteerTypes
        {
            get { return context.VolunteerTypes; }
        }

        public IQueryable<ClusterCoordinator> ClusterCoordinators
        {
            get { return context.ClusterCoordinators; }
        }

        public IQueryable<Commitment> Commitments
        {
            get { return context.Commitments; }
        }

        public IQueryable<Disaster> Disasters
        {
            get { return context.Disasters; }
        }

        public IQueryable<DisasterCluster> DisasterClusters
        {
            get { return context.DisasterClusters; }
        }

        public IQueryable<Person> Persons
        {
            get { return context.Persons; }
        }

        public IQueryable<User> Users
        {
            get { return membership_context.Users; }
        }

        public IQueryable<ClusterCoordinatorLogEntry> ClusterCoordinatorLogEntries
        {
            get { return context.ClusterCoordinatorLogEntries; }
        }

        public Person AddPerson(Person newPerson)
        {
            var result = context.Persons.Add(newPerson);
            context.SaveChanges();
            return result;
        }

        public Person UpdatePerson(Person updatedPerson)
        {
            var result = context.Persons.Find(updatedPerson.Id);

            if (result == null)
                throw new PersonNotFoundException();

            result.FirstName = updatedPerson.FirstName;
            result.LastName = updatedPerson.LastName;
            result.Email = updatedPerson.Email;
            result.PhoneNumber = updatedPerson.PhoneNumber;

            context.SaveChanges();

            return result;
        }

        public void AddCommitment(Commitment newCommitment)
        {
            context.Commitments.Add(newCommitment);
            context.SaveChanges();
        }

        public void RemoveCommitmentById(int id)
        {
            var commitment = context.Commitments.Find(id);
            context.Commitments.Remove(commitment);
            context.SaveChanges();
        }

        public Commitment UpdateCommitment(Commitment updatedCommitment)
        {
            var result = context.Commitments.Find(updatedCommitment.Id);

            if (result == null)
                throw new CommitmentNotFoundException();

            result.StartDate = updatedCommitment.StartDate;
            result.EndDate = updatedCommitment.EndDate;
            result.Status = updatedCommitment.Status;
            result.PersonIsCheckedIn = updatedCommitment.PersonIsCheckedIn;
            result.VolunteerType = updatedCommitment.VolunteerType;
            result.ClusterId = updatedCommitment.ClusterId;

            context.SaveChanges();

            return result;
        }

        public void AddDisaster(Disaster newDisaster)
        {
            context.Disasters.Add(newDisaster);
            context.SaveChanges();
        }

        public void AddCluster(Cluster newCluster)
        {
            context.Clusters.Add(newCluster);
            context.SaveChanges();
        }

        public void RemoveCluster(Cluster cluster)
        {
            // attach the cluster to delete to the context, if needed. Otherwise the remove/delete won't work
            var cluster2Del = context.Clusters.Local.FirstOrDefault(cc => cc.Id == cluster.Id);
            if (cluster2Del == null)
                context.Clusters.Attach(cluster);

            context.Clusters.Remove(cluster);
            context.SaveChanges();
        }

        
        public Disaster UpdateDisaster(Disaster updatedDisaster)
        {
            var result = context.Disasters.Find(updatedDisaster.Id);

            if (result == null)
                throw new DisasterNotFoundException();

            result.Name = updatedDisaster.Name;
            result.IsActive = updatedDisaster.IsActive;

            context.SaveChanges();

            return result;
        }

        public void AddDisasterCluster(DisasterCluster newDisasterCluster)
        {
            context.DisasterClusters.Add(newDisasterCluster);
            context.SaveChanges();
        }

        public void RemoveDisasterCluster(DisasterCluster newDisasterCluster)
        {
            context.DisasterClusters.Remove(newDisasterCluster);
            context.SaveChanges();
        }

        public Cluster UpdateCluster(Cluster updatedCluster)
        {
            var result = context.Clusters.Find(updatedCluster.Id);

            if (result == null)
                throw new ClusterNotFoundException();

            result.Name = updatedCluster.Name;

            context.SaveChanges();

            return result;
        }
        
        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public ClusterCoordinator AddClusterCoordinator(ClusterCoordinator clusterCoordinator)
        {
            context.ClusterCoordinators.Add(clusterCoordinator);
            context.SaveChanges();
            return clusterCoordinator;
        }

        public void RemoveClusterCoordinator(ClusterCoordinator clusterCoordinator)
        {
            // attach the coordinator to delete to the context, if needed. Otherwise the
            // remove/delete won't work
            var coordinatorToDelete = context.ClusterCoordinators.Local.FirstOrDefault(cc => cc.Id == clusterCoordinator.Id);
            if (coordinatorToDelete == null)
                context.ClusterCoordinators.Attach(clusterCoordinator);

            context.ClusterCoordinators.Remove(coordinatorToDelete);
            context.SaveChanges();
        }

       
        public void AppendClusterCoordinatorLogEntry(ClusterCoordinatorLogEntry clusterCoordinatorLogEntry)
        {
            context.ClusterCoordinatorLogEntries.Add(clusterCoordinatorLogEntry);
            context.SaveChanges();
        }
    }
}