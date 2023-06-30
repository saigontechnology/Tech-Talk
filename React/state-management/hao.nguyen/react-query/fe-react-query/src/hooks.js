/* eslint-disable react-hooks/rules-of-hooks */
import { useQuery, useMutation, useQueries } from "@tanstack/react-query";
import {
  getNotes,
  createNote,
  updateNote,
  deleteNote,
  getNoteById,
} from "./services";

export const useGetNotes = () => {
  const { data, ...rest } = useQuery(["notes"], getNotes, {
    retry: 5,
    retryDelay: 5000,
    staleTime: 10000,
  });

  return {
    ...rest,
    data,
  };
};

export const useGetNoteById = (id) => {
  const { data, ...rest } = useQuery([`note ${id}`], () => getNoteById(id), {
    retry: 1,
    retryDelay: 1000,
    cacheTime: 5000,
  });

  return {
    ...rest,
    data,
  };
};

export const useGetMultiApi = () => {
  const { data, ...rest } = useQueries({
    queryKey: ["notes"],
    queryFn: () => getNotes(),
  });

  return {
    ...rest,
    data,
  };
};

export const useCreateNote = () => {
  const handleCreateNote = async (data) => {
    return await createNote(data);
  };
  return useMutation(handleCreateNote);
};

export const useUpdateNote = () => {
  const handleUpdateNote = async (data) => {
    return await updateNote(data);
  };
  return useMutation(handleUpdateNote);
};

export const useDeleteNote = () => {
  const handleDeleteNote = async (noteId) => {
    return await deleteNote(noteId);
  };
  return useMutation(handleDeleteNote);
};
