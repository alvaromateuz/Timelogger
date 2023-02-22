import React, { useState, useEffect } from 'react';
import { getTimelogs } from '../api/timelogs';

interface Timelog {
  timeLogId: number;
  projectName: string;
  customerName: string;
  developerName: string;
  timeSpent: number;
  deadLine: string;
  logDate: string;
  description: string;
}

interface Pagination{
  pageIndex: number;
  totalPages: number;
}

const TimeLogsTable: React.FC<{projectId?: number, developerId?: number }> = ({projectId, developerId }) => {
  
  const [timelogs, setTimelogs] = useState<Timelog[]>([]);
  const [pagination, setPagination] = useState<Pagination>({pageIndex:1,totalPages:1});
  
  const fetchTimelogs = async (pageIndex: number) => {
    try {
      const response = await getTimelogs(pageIndex, 10, projectId, developerId);

      if (response.items) {
        setTimelogs(response.items);
      } 
      else {
        setTimelogs([]);
      }

      setPagination(prevState => ({
        ...prevState,
        totalPages: response.totalPages
      }));

    } catch (error) {
      console.error(error);
      setTimelogs([]);
    }
  };

  useEffect(() => {
    fetchTimelogs(pagination.pageIndex);
  }, [projectId, developerId, pagination.pageIndex]);

  return (
    <>
      <table className='table-fixed w-full'>
        <thead className='bg-gray-200'>
          <tr>
            <th className='border px-4 py-2 w-40'>Timelog ID</th>
            <th className='border px-4 py-2 w-40'>Customer</th>
            <th className='border px-4 py-2'>Project</th>
            <th className='border px-4 py-2'>Description</th>
            <th className='border px-4 py-2 w-40'>Developer</th>
            <th className='border px-4 py-2 w-40'>Time Spent</th>
            <th className='border px-4 py-2 w-40'>Deadline</th>
            <th className='border px-4 py-2 w-40'>Log Date</th>
          </tr>
        </thead>
        <tbody>
          {timelogs.map((timelog) => (
            <tr key={timelog.timeLogId}>
              <td className={'bg-gray-50 border px-4 py-2'}>{timelog.timeLogId}</td>
              <td className={'bg-gray-50 border px-4 py-2'}>{timelog.customerName}</td>
              <td className={'bg-gray-50 border px-4 py-2'}>{timelog.projectName}</td>
              <td className={'bg-gray-50 border px-4 py-2'}>{timelog.description}</td>
              <td className={'bg-gray-50 border px-4 py-2'}>{timelog.developerName}</td>
              <td className={'bg-gray-50 border px-4 py-2'}>{timelog.timeSpent}</td>
              <td className={'bg-gray-50 border px-4 py-2'}>{new Date(timelog.deadLine).toLocaleDateString()}</td>
              <td className={'bg-gray-50 border px-4 py-2'}>{new Date(timelog.logDate).toLocaleDateString()}</td>
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

export default TimeLogsTable;