import axiosRequest from './axiosRequest';

const API_URL = 'http://localhost:3600';

export const getNotes = async (data) => {
  const result = await axiosRequest({
    method: 'GET',
    data,
    url: `${API_URL}/notes`,
  });

  return result;
};

export const getNoteById = async (id) => {
  const result = await axiosRequest({
    method: 'GET',
    url: `${API_URL}/notes/${id}`,
  });

  return result;
};

export const createNote = async (data) => {
  const result = await axiosRequest({
    method: 'POST',
    data,
    url: `${API_URL}/notes`,
  });

  return result;
};

export const updateNote = async (data) => {
  const result = await axiosRequest({
    method: 'PUT',
    data,
    url: `${API_URL}/notes/${data?.id}`,
  });

  return result;
};

export const deleteNote = async (id) => {
  const result = await axiosRequest({
    method: 'DELETE',
    url: `${API_URL}/notes/${id}`,
  });

  return result;
};
