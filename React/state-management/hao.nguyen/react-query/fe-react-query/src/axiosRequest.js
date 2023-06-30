import axios from 'axios';

const axiosRequest = async ({ method, data, url }) => {
  try {
    const response = await axios({
      method,
      url,
      data,
    });

    return response.data;
  } catch (error) {
    // Handle error or throw it for the caller to handle
    throw error;
  }
};

export default axiosRequest;
