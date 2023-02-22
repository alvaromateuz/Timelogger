import React, { useEffect, useState } from "react";
import ProjectsTable from "../components/Projects";

export default function Projects() {
    
    const [formData, setFormData] = useState({
        projectName: '',
        projectStageName: '',
        deadline: new Date().toISOString().slice(0, 16),
        customerName: '',
        projectId: 0,
        projectStageId: 0,
        customerId: 0,
      });

      //Customers
      interface Customer {
        customerId: number;
        customerName: string;
      }
      
      const [customers, setCustomers] = useState<Customer[]>([]);
    
      useEffect(() => {
        fetch('http://localhost:3001/v1/Customer?pageIndex=1&pageSize=100')
          .then(response => response.json())
          .then(data => setCustomers(data.items))
          .catch(error => console.error(error));
      }, []);

      //Project Stages
      interface ProjectStage {
          projectStageId: number;
          projectStageName: string;
        }
  
      const [projectStages, setProjectStages] = useState<ProjectStage[]>([]);
    
      useEffect(() => {
            fetch('http://localhost:3001/v1/ProjectStage?pageIndex=1&pageSize=100')
            .then(response => response.json())
            .then(data => setProjectStages(data.items))
            .catch(error => console.error(error));
        }, []);
    
      //Add Project
      const [error, setError] = useState<null | string>('');
      
      const [showModal, setShowModal] = useState({
        show: false,
        mode: "add"
      });

      function handleFormSubmit(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();

        const url = showModal.mode === 'add' ? 'http://localhost:3001/v1/Project' : `http://localhost:3001/v1/Project?id=${formData.projectId}`;
        const method = showModal.mode === 'add' ? 'POST' : 'PUT';
    
        fetch(url, {
          method: method,
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify(formData),
        }).then(async (response) => {
          
          if (response.status && response.status !== 200) {
            event.preventDefault();
            const errorMessage = await response.text();
            setError(errorMessage.replace(/["\[\]{}]/g, " "));
          }else {
            window.location.reload();
            setError(null);
          }
        })
        .catch(async () => {
          event.preventDefault();
            setError('Please review the data and try again, if the problem persists, contact our support team');
        });
      }

      function handleEdit(projectId: number, projectName: string, projectStageId: number, deadline: string, customerId: number){
        setShowModal({show: true, mode: "edit"});
        setFormData(prevState => ({
          ...prevState, projectName: projectName, projectStageId: projectStageId, deadline: deadline, customerId: customerId, projectId: projectId}));
      }



    return (
        <>
            <div className="flex items-center my-6">
                <div className="w-1/2">
                    <button
                        className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
                        onClick={() => setShowModal({show: true, mode: "add"})}
                    >
                    New Project
                    </button>
                </div>
            </div>

            <div className={`fixed z-10 inset-0 overflow-y-auto ${showModal.show ? 'block' : 'hidden'}`}>
                <div className="flex items-center justify-center min-h-screen">
                    <div className="bg-gray-100 border rounded-full rounded-lg overflow-hidden w-1/2">
                        <div className="px-6 py-4">
                            <h2 className="text-lg font-bold mb-4">
                            {showModal.mode=="edit" ? 'Edit Project #' + formData.projectId.toString() : 'Add Project'}
                            </h2>

                            <form onSubmit={handleFormSubmit}>
                              	

                                <div className="mb-4">
                                    <label className="block text-gray-700 font-bold mb-2" htmlFor="projectName">
                                    Project Name
                                    </label>
                                    <input
                                    type="text"
                                    name="projectName"
                                    id="projectName"
                                    value={formData.projectName}
                                    onChange={(event) =>
                                        setFormData({
                                        ...formData,
                                        projectName: event.target.value,
                                        })
                                    }
                                    className="border rounded-full py-2 px-4"
                                    />
                                </div>

                                <div className="mb-4">
                                    <label className="block text-gray-700 font-bold mb-2" htmlFor="projectStageId">
                                    Project Stage
                                    </label>
                                    <select
                                    name="projectStageId"
                                    id="projectStageId"
                                    value={formData.projectStageId}
                                    onChange={(event) =>
                                        setFormData({
                                        ...formData,
                                        projectStageId: parseInt(event.target.value),
                                        })
                                    }
                                    className="border rounded-full py-2 px-4"
                                    >
                                    <option value="">Select Stage</option>
                                    {projectStages.map((projectStage) => (
                                        <option key={projectStage.projectStageId} value={projectStage.projectStageId}>
                                        {projectStage.projectStageName}
                                        </option>
                                    ))}
                                    </select>
                                </div>

                                <div className="mb-4">
                                    <label className="block text-gray-700 font-bold mb-2" htmlFor="deadline">
                                    Deadline
                                    </label>
                                    <input
                                    type="datetime-local"
                                    name="deadline"
                                    id="deadline"
                                    value={formData.deadline}
                                    onChange={(event) =>
                                        setFormData({
                                        ...formData,
                                        deadline: event.target.value,
                                        })
                                    }
                                    className="border rounded-full py-2 px-4"
                                    />
                                </div>

                                <div className="mb-4">
                                    <label className="block text-gray-700 font-bold mb-2" htmlFor="customerId">
                                    Customer
                                    </label>
                                    <select
                                    name="customerId"
                                    id="customerId"
                                    value={formData.customerId}
                                    onChange={(event) =>
                                        setFormData({
                                        ...formData,
                                        customerId: parseInt(event.target.value),
                                        })
                                    }
                                    className="border rounded-full py-2 px-4"
                                    >
                                    <option value="">Select Customer</option>
                                    {customers.map((customer) => (
                                        <option key={customer.customerId} value={customer.customerId}>
                                        {customer.customerName}
                                        </option>
                                    ))}
                                    </select>
                                </div>

                                <div className="flex justify-end">

                                    {error && (
                                      <div className="text-red-500 mt-2 pr-5">
                                        {error}
                                      </div>
                                    )}

                                    <button className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded mr-2">
                                    Save
                                    </button>
                                    <button className="bg-gray-500 hover:bg-gray-700 text-white font-bold py-2 px-4 rounded"
                                    onClick={(e) => {
                                        e.preventDefault(); 
                                        setShowModal({show: false, mode: "add"});
                                        setError(null);
                                        setFormData(prevState => ({
                                          ...prevState, projectName: '', projectStageId: 0, deadline: new Date().toISOString().slice(0, 16), customerId: 0, projectId: 0 }));
                                        }}
                                        >
                                    Cancel
                                    </button>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
            </div>

            <div className="my-6">
                <h1 className="text-xl font-bold mb-2">Projects</h1>
                <ProjectsTable onEdit={handleEdit} />
            </div>


        </>
    );
}
