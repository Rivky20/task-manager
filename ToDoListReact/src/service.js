import axios from 'axios';

axios.defaults.baseURL = process.env.REACT_APP_API_URL; 

axios.interceptors.response.use(
  response => response, 
  error => {
    console.error("API Error:", error.response ? error.response.data : error.message);
    return Promise.reject(error); 
  }
);

export default {
  getTasks: async () => {
    const result = await axios.get("/items");  
    return result.data;
  },

  addTask: async (name) => {
    const result = await axios.post("/items", { name });  
    return result.data;
  },

  setCompleted: async (id, isComplete, name) => {
    const result = await axios.put(`/items/${id}`, { 
      isComplete,
      Name: name
    });
    return result.data;
  },

  deleteTask: async (id) => {
    await axios.delete(`/items/${id}`);  
    return { message: 'Task deleted successfully' };
  }
};