import axios from 'axios';

export const getTimelogs = async (pageIndex: number, pageSize: number, projectId?: number, developerId?: number, initialDate?: string, finalDate?: string) => {
  const response = await axios.get(`http://localhost:3001/v1/Timelog/search?ProjectId=${projectId}&DeveloperId=${developerId}&InitialDate=${initialDate}&FinalDate=${finalDate}&pageIndex=${pageIndex}&pageSize=${pageSize}`);
  return response.data;
};
