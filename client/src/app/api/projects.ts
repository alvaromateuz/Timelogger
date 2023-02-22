import axios from 'axios';

export const getProjects = async (pageIndex: number, pageSize: number, sortBy: string, sortDirection: string) => {
  const response = await axios.get(`http://localhost:3001/v1/Project?pageIndex=${pageIndex}&pageSize=${pageSize}&sortBy=${sortBy}&sortDirection=${sortDirection}`);
  return response.data;
};
