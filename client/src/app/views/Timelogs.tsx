import React, { useEffect, useState } from "react";
import TimeLogsTable from "../components/TimeLogs";

export default function TimeLogs() {
    const [showModal, setShowModal] = useState(false);

    const [formData, setFormData] = useState({
        projectId: 0,
        developerId: 0,
        timeSpent: 0,
        logDate: new Date().toISOString().slice(0, 16),
        description: ''
      });
      
      //Get Developers
      interface Developer {
          developerId: number;
          developerName: string;
        }
      const [developers, setDevelopers] = useState<Developer[]>([]);
      useEffect(() => {
            fetch('http://localhost:3001/v1/Developer?pageIndex=1&pageSize=100')
            .then(response => response.json())
            .then(data => setDevelopers(data.items))
            .catch(error => console.error(error));
        }, []);
      
      //Get Projects
      interface Project {
        projectId: number;
        projectName: string;
      }
      const [projects, setProjects] = useState<Project[]>([]);
      useEffect(() => {
            fetch('http://localhost:3001/v1/Project?pageIndex=1&pageSize=100')
            .then(response => response.json())
            .then(data => setProjects(data.items))
            .catch(error => console.error(error));
        }, []);

      //Add Timelog
      const [error, setError] = useState<null | string>('');
      function handleFormSubmit(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();
    
        fetch('http://localhost:3001/v1/TimeLog', {
          method: 'POST',
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
                setError('Please review and try again, if the problem persists, contact our support team');
            });
      };

      //Filters
      const [projectId, setProjectId] = useState<number>();
      const [developerId, setDeveloperId] = useState<number>();
      

    return (
        <>
            <div className="flex items-center my-6">
                <div className="w-1/2">
                    <button
                        className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
                        onClick={() => setShowModal(true)}
                    >
                        Log it!
                    </button>
                </div>

                <div className="w-1/2 flex justify-end">
                    <form>
                        <select
                          name="projectId"
                          id="projectId"
                          value={projectId}
                          onChange={(event) =>
                            setProjectId(parseInt(event.target.value))
                          }
                          className="border rounded-full py-2 px-4 mr-4"
                          >
                          <option value="">Select Project</option>
                          {projects.map((project) => (
                              <option key={project.projectId} value={project.projectId}>
                              {project.projectName}
                              </option>
                          ))}
                        </select>

                        <select
                          name="developerId"
                          id="developerId"
                          value={developerId}
                          onChange={(event) =>
                            setDeveloperId(parseInt(event.target.value))
                          }
                          className="border rounded-full py-2 px-4 mr-4"
                          >
                          <option value="">Select Developer</option>
                          {developers.map((developer) => (
                              <option key={developer.developerId} value={developer.developerId}>
                              {developer.developerName}
                              </option>
                          ))}
                        </select>
                        
                        <button
                        onClick={ ()=>{setProjectId(undefined); setDeveloperId(undefined);}}
                        className="bg-blue-500 hover:bg-blue-700 text-white rounded-full py-2 px-4 ml-2"
                        type="submit"
                        >
                        Remove Filters
                        </button>
                    </form>
                </div>
            </div>

            

            <div className={`fixed z-10 inset-0 overflow-y-auto ${showModal ? 'block' : 'hidden'}`}>
                <div className="flex items-center justify-center min-h-screen">
                    <div className="bg-gray-100 border rounded-full rounded-lg overflow-hidden w-1/2">
                        <div className="px-6 py-4">
                            <h2 className="text-lg font-bold mb-4">Add Time log</h2>

                            <form onSubmit={handleFormSubmit}>
                                <div className="mb-4">
                                    <label className="block text-gray-700 font-bold mb-2" htmlFor="projectName">
                                    Project
                                    </label>
                                    <select
                                    name="projectId"
                                    id="projectId"
                                    value={formData.projectId}
                                    onChange={(event) =>
                                        setFormData({
                                        ...formData,
                                        projectId: parseInt(event.target.value),
                                        })
                                    }
                                    className="border rounded-full py-2 px-4"
                                    >
                                    <option value="">Select Project</option>
                                    {projects.map((project) => (
                                        <option key={project.projectId} value={project.projectId}>
                                        {project.projectName}
                                        </option>
                                    ))}
                                    </select>
                                </div>

                                <div className="mb-4">
                                    <label className="block text-gray-700 font-bold mb-2" htmlFor="developerId">
                                    Developer
                                    </label>
                                    <select
                                    name="developerId"
                                    id="developerId"
                                    value={formData.developerId}
                                    onChange={(event) =>
                                        setFormData({
                                        ...formData,
                                        developerId: parseInt(event.target.value),
                                        })
                                    }
                                    className="border rounded-full py-2 px-4"
                                    >
                                    <option value="">Select Developer</option>
                                    {developers.map((developer) => (
                                        <option key={developer.developerId} value={developer.developerId}>
                                        {developer.developerName}
                                        </option>
                                    ))}
                                    </select>
                                </div>

                                <div className="mb-4">
                                    <label className="block text-gray-700 font-bold mb-2" htmlFor="logDate">
                                    Log Date
                                    </label>
                                    <input
                                    type="datetime-local"
                                    name="logDate"
                                    id="logDate"
                                    value={formData.logDate}
                                    onChange={(event) =>
                                        setFormData({
                                        ...formData,
                                        logDate: event.target.value,
                                        })
                                    }
                                    className="border rounded-full py-2 px-4"
                                    />
                                </div>

                                <div className="mb-4">
                                    <label className="block text-gray-700 font-bold mb-2" htmlFor="timeSpent">
                                    Time Spent
                                    </label>
                                    <input
                                    type="text"
                                    name="timeSpent"
                                    id="timeSpent"
                                    value={formData.timeSpent}
                                    onChange={(event) =>
                                        setFormData({
                                        ...formData,
                                        timeSpent: parseInt(event.target.value),
                                        })
                                    }
                                    className="border rounded-full py-2 px-4"
                                    />
                                </div>

                                <div className="mb-4">
                                    <label className="block text-gray-700 font-bold mb-2" htmlFor="description">
                                    Description
                                    </label>
                                    <input
                                    type="text"
                                    name="description"
                                    id="description"
                                    value={formData.description}
                                    onChange={(event) =>
                                        setFormData({
                                        ...formData,
                                        description: event.target.value,
                                        })
                                    }
                                    className="border rounded-full py-2 px-4"
                                    />
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
                                        setShowModal(false);
                                        setError(null);
                                        setFormData({ timeSpent: 0, projectId: 0, logDate: new Date().toISOString().slice(0, 16), developerId: 0, description:'' })}}
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
                <h1 className="text-xl font-bold mb-2">Time Logs</h1>
                
                <TimeLogsTable projectId={projectId} developerId={developerId} />

            </div>

        </>
    );
}
