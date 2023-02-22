import React, { useState, useEffect } from 'react';
import { getProjects } from '../api/projects';

interface Project {
  projectId: number;
  projectName: string;
  projectStageId: number;
  customerId: number;
  deadline: string;
  projectStageName: string;
  customerName: string;
}

interface ProjectsTableProps {
  onEdit: (projectId: number, projectName: string, projectStageId: number, deadline: string, customerId: number) => void;
}

interface Pagination{
  pageIndex: number;
  totalPages: number;
}

const ProjectsTable: React.FC<ProjectsTableProps> = ({onEdit}) => {
  const [projects, setProjects] = useState<Project[]>([]);
  const [sortField, setSortField] = useState<string>("deadline");
  const [sortOrder, setSortOrder] = useState<string>("asc");
  const [pagination, setPagination] = useState<Pagination>({pageIndex:1,totalPages:1});

  const fetchProjects = async (pageIndex: number) => {
    
    try{
      const response = await getProjects(pageIndex, 10, sortField, sortOrder);
      
      if (response.items) {
        setProjects(response.items);
      } 
      else {
        setProjects([]);
      }

      setPagination(prevState => ({
        ...prevState,
        totalPages: response.totalPages
      }));

    } catch (error) {
      console.error(error);
      setProjects([]);
    }

  };

  useEffect(() => {
    fetchProjects(pagination.pageIndex);
  }, [sortField, sortOrder, pagination.pageIndex]);

  const handleSort = (field: string) => {
    if (field === sortField) {
      setSortOrder(sortOrder === "asc" ? "desc" : "asc");
    } else {
      setSortField(field);
      setSortOrder("asc");
    }
  };


  return (
    <>
      <table className='table-fixed w-full'>
        <thead className='bg-gray-200'>
          <tr>
            <th className='border px-4 py-2 w-40 '>Project ID</th>
            <th className='border px-4 py-2 cursor-pointer' onClick={() => handleSort("projectName")}>Project Name ↑↓</th>
            <th className='border px-4 py-2 w-40'>Stage</th>
            <th className='border px-4 py-2 w-40'>Customer</th>
            <th className='border px-4 py-2 w-40 cursor-pointer' onClick={() => handleSort("deadline")}>Deadline ↑↓</th>
            <th className='border px-4 py-2 w-20'></th>
          </tr>
        </thead>
        <tbody>
          {projects.map((project) => (
            <tr key={project.projectId}>
              <td className={'bg-gray-50 border px-4 py-2'}>{project.projectId}</td>
              <td className={'bg-gray-50 border px-4 py-2'}>{project.projectName}</td>
              <td className={'bg-gray-50 border px-4 py-2'}>{project.projectStageName}</td>
              <td className={'bg-gray-50 border px-4 py-2'}>{project.customerName}</td>
              <td className={'bg-gray-50 border px-4 py-2'}>{new Date(project.deadline).toLocaleDateString()}</td>
              <td className={'bg-gray-50 border px-4 py-2 cursor-pointer text-blue-600 hover:underline'} onClick={() => onEdit(project.projectId,project.projectName,project.projectStageId,project.deadline,project.customerId)}>Edit</td>
            </tr>
          ))}
        </tbody>
      </table>
      
      <div className="text-center my-6 px-4">
          <button className="bg-gray-500 hover:bg-gray-700 text-white font-bold py-2 px-4 mx-4 rounded"
              onClick={() => {
                if (pagination.pageIndex > 1) {
                  setPagination(prevState => ({
                    ...prevState,
                    pageIndex: prevState.pageIndex - 1
                  }));
                }
              }}
                  >
              &lt;&lt;
          </button>

          Page {pagination.pageIndex} / {pagination.totalPages}

          <button className="bg-gray-500 hover:bg-gray-700 text-white font-bold py-2 px-4 mx-4 rounded"
              onClick={() => {
                if (pagination.pageIndex < pagination.totalPages) {
                  setPagination(prevState => ({
                    ...prevState,
                    pageIndex: prevState.pageIndex + 1
                  }));
                }
              }}
                  >
              &gt;&gt;
          </button>
      </div>
    
    </>
  );
};

export default ProjectsTable;