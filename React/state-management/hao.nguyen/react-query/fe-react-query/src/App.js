import React, { useEffect, useState } from "react";
import { Form, Input, Button, Table, Card, Space } from "antd";
import { EditOutlined, DeleteOutlined } from "@ant-design/icons";
import {
  useCreateNote,
  useDeleteNote,
  useGetNoteById,
  useGetNotes,
  useUpdateNote,
} from "./hooks";
import { useQueryClient } from "@tanstack/react-query";

const { Column } = Table;

const App = () => {
  const [form] = Form.useForm();
  const [isEdit, setIsEdit] = useState(false);
  const [noteSelectedId, setNoteSelectedId] = useState(null);

  const {
    data: notes,
    refetch: refetchNotes,
    isLoading: isGetNotesLoading,
    isError,
    isSuccess,
    isFetching,
    status
  } = useGetNotes();
  const {
    data: noteDetail,
    refetch: refetchNoteDetail,
    isLoading: isGetNoteByIdLoading,
  } = useGetNoteById(noteSelectedId);

  const { mutate: createNoteMutate, isLoading: isCreateNoteLoading } =
    useCreateNote();
  const { mutate: updateNoteMutate, isLoading: isUpdateNoteLoading } =
    useUpdateNote();
  const { mutate: deleteNoteMutate } =
    useDeleteNote();

  useEffect(() => {
    refetchNoteDetail();
  }, [noteSelectedId, refetchNoteDetail]);

  useEffect(() => {
    if (noteDetail) {
      form.setFieldValue("title", noteDetail?.title);
      form.setFieldValue("content", noteDetail?.content);
    }
  }, [form, noteDetail]);

  const onFinish = (values) => {
    if (isEdit) {
      updateNoteMutate({ ...values, id: noteSelectedId }, {
        onSuccess: () => {
          refetchNotes();
          setIsEdit(false);
          form.resetFields();
        },
        onError: () => { },
      });
    } else {
      createNoteMutate(values, {
        onSuccess: () => {
          refetchNotes();
          setIsEdit(false);
          form.resetFields();
        },
        onError: () => { },
      });
    }

  };

  const handleDelete = (id) => {
    deleteNoteMutate(id, {
      onSuccess: () => {
        refetchNotes();
      },
      onError: () => { },
    });
  };

  // Get data from query key
  const queryClient = useQueryClient();
  const queryKey = ['notes'];

  const notesCache = queryClient.getQueryData(queryKey);

  // console.log('notes', notes);

  // Remove
  const handleRemoveDataCache = () => {
    queryClient.removeQueries(queryKey);
  }

  console.log('notesCache', notesCache);

  // invalid query
  // queryClient.invalidateQueries(queryKey);

  // change value cache
  // queryClient.setQueryData(queryKey, []);

  return (
    <div style={{ padding: "100px" }}>
      <h1 style={{ textAlign: "center", marginBottom: "24px" }}>
        Note Taking App
      </h1>
      <Card style={{ padding: "24px" }}>
        <h3>{isGetNoteByIdLoading && 'Loading note to edit ...'}</h3>
        <Form form={form} layout="vertical" onFinish={onFinish}>
          <Form.Item
            label="Title"
            name="title"
            rules={[{ required: true, message: "Please enter a title" }]}
          >
            <Input size="large" />
          </Form.Item>
          <Form.Item
            label="Content"
            name="content"
            rules={[{ required: true, message: "Please enter content" }]}
          >
            <Input.TextArea rows={4} />
          </Form.Item>
          <Form.Item>
            <Button
              style={{ marginRight: "10px" }}
              type="primary"
              htmlType="submit"
              loading={isCreateNoteLoading || isUpdateNoteLoading}
            >
              Save
            </Button>
            <Button type="default" onClick={() => form.resetFields()}>
              Clear
            </Button>
          </Form.Item>
        </Form>
      </Card>
      {/* <h1>{notesCache?.length && notesCache?.[0]?.title}</h1> */}
      <div style={{ marginTop: "24px" }}>
        <Button type="default" onClick={() => refetchNotes()}>
          Refresh
        </Button>
        <Button style={{ marginLeft: 10 }} type="primary" onClick={() => handleRemoveDataCache()}>
          Reset
        </Button>
        <Table dataSource={notes} loading={isGetNotesLoading} style={{ marginTop: 5}} pagination >
          <Column title="Title" dataIndex="title" key="title" />
          <Column title="Content" dataIndex="content" key="content" />
          <Column
            title="Action"
            key="action"
            render={(text, record) => (
              <Space>
                <Button
                  type="text"
                  icon={<EditOutlined />}
                  onClick={() => {
                    setNoteSelectedId(record._id);
                    setIsEdit(true);
                  }}
                />
                <Button
                  type="text"
                  icon={<DeleteOutlined />}
                  onClick={() => handleDelete(record._id)}
                />
              </Space>
            )}
          />
        </Table>
      </div>
    </div>
  );
};

export default App;
