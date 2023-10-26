import React, { useState, useEffect } from 'react';
import { Table, Space, Button, Modal, Form, Input, Card } from 'antd';
import { EditOutlined, DeleteOutlined } from '@ant-design/icons';
import axios from 'axios';

const { Column } = Table;

const UserManagement = () => {
  const [dataSource, setDataSource] = useState([]);
  const [dataWebhookReceiverSource, setDataWebhookReceiverSource] = useState([]);
  const [form] = Form.useForm();
  const [isEditModalVisible, setIsEditModalVisible] = useState(false);
  const [isEditModalWebhookReceiverVisible, setIsEditModalWebhookReceiverVisible] = useState(false);
  const [isDeleteModalVisible, setIsDeleteModalVisible] = useState(false);
  const [isDeleteModalWebhookReceiverVisible, setIsDeleteModalWebhookReceiverVisible] = useState(false);
  const [selectedUser, setSelectedUser] = useState(null);
  const [actionType, setActionType] = useState(null);
  const [selectedReceiverWebhook, setSelectedReceiverWebhook] = useState(null);

  useEffect(() => {
    // Fetch the user list from your API when the component mounts
    axios.get('http://localhost:9000/api/users')
      .then((response) => {
        setDataSource(response.data);
      })
      .catch((error) => {
        console.error('Error fetching user list:', error);
      });

    // Fetch the receiver webhook list from your API when the component mounts
    axios.get('http://localhost:9000/api/webhooks-receiver')
      .then((response) => {
        setDataWebhookReceiverSource(response.data);
      })
      .catch((error) => {
        console.error('Error fetching receiver webhook list:', error);
      });
  }, []);

  const fetchWebhooksReceiver = () => {
    axios.get('http://localhost:9000/api/webhooks-receiver')
    .then((response) => {
      setDataWebhookReceiverSource(response.data);
    })
    .catch((error) => {
      console.error('Error fetching receiver webhook list:', error);
    });
  }

  const showEditModal = (user) => {
    setSelectedUser(user);
    setIsEditModalVisible(true);
    setActionType('edit');
    form.setFieldsValue(user);
  };

  const showAddModal = () => {
    setIsEditModalVisible(true);
    setActionType('add');
  }

  const showAddWebhooksReceiverModal = () => {
    setIsEditModalWebhookReceiverVisible(true);
    setActionType('add');
  }

  const showDeleteModal = (user) => {
    setSelectedUser(user);
    setIsDeleteModalVisible(true);
  };

  const showDeleteWebhookReceiverModal = (receiver) => {
    setSelectedReceiverWebhook(receiver);
    setIsDeleteModalWebhookReceiverVisible(true);
  };

  const handleEditCancel = () => {
    setIsEditModalVisible(false);
    setSelectedUser(null);
  };

  const handleEditWebhooksReceiverCancel = () => {
    setIsEditModalWebhookReceiverVisible(false);
    setSelectedReceiverWebhook(null);
  };

  const handleDeleteCancel = () => {
    setIsDeleteModalVisible(false);
    setSelectedUser(null);
  };

  const handleEdit = (values) => {
    axios
      .put(`http://localhost:9000/api/users/${selectedUser.id}`, values)
      .then(() => {
        const updatedDataSource = dataSource.map((user) =>
          user.id === selectedUser.id ? { ...user, ...values } : user
        );
        setDataSource(updatedDataSource);
        form.resetFields();
        setIsEditModalVisible(false);
        setSelectedUser(null);
      })
      .catch((error) => {
        console.error('Error editing user:', error);
      });
  };

  const handleDelete = () => {
    axios.delete(`http://localhost:9000/api/users/${selectedUser.id}`)
      .then(() => {
        const updatedDataSource = dataSource.filter(
          (user) => user.id !== selectedUser.id
        );
        setDataSource(updatedDataSource);
        setIsDeleteModalVisible(false);
        setSelectedUser(null);
      })
      .catch((error) => {
        console.error('Error deleting user:', error);
      });
  };

  const handleAddUser = (values) => {
    axios.post('http://localhost:9000/api/users', values)
      .then((response) => {
        const newUser = {
          key: response.data.id, // Assuming your API returns a unique ID for the new user
          ...values,
        };
        setDataSource([...dataSource, newUser]);
        form.resetFields();
        setIsEditModalVisible(false);
      })
      .catch((error) => {
        console.error('Error adding user:', error);
      });
  };

  const handleDeleteReceiver = () => {
    axios.delete(`http://localhost:9000/api/webhooks-receiver/${selectedReceiverWebhook.id}`)
      .then(() => {
        const updatedDataSource = dataSource.filter(
          (receiverWebhook) => receiverWebhook.id !== selectedReceiverWebhook.id
        );
        // setDataWebhookReceiverSource(updatedDataSource);
        fetchWebhooksReceiver();
        setIsDeleteModalWebhookReceiverVisible(false);
        setSelectedReceiverWebhook(null);
      })
      .catch((error) => {
        console.error('Error deleting receiver webhook:', error);
      });
  };

  const handleAddReceiverWebhook = (values) => {
    axios.post('http://localhost:9000/api/webhooks-receiver', values)
      .then((response) => {
        const newReceiverWebhook = {
          id: response.data.id, // Assuming your API returns a unique ID for the new receiver webhook
          ...values,
        };
        // setDataWebhookReceiverSource([...dataSource, newReceiverWebhook]);
        // form.resetFields()9;
        fetchWebhooksReceiver();
        setIsEditModalWebhookReceiverVisible(false);
      })
      .catch((error) => {
        console.error('Error adding receiver webhook:', error);
      });
  };

  const handleDeleteReceiverCancel = () => {
    setIsDeleteModalWebhookReceiverVisible(false);
    setSelectedReceiverWebhook(null);
  };

  return (
    <Card style={{ margin: 50 }}>
      <h1>User Management</h1>
      <Button type="primary" onClick={showAddModal}>
        Add New User
      </Button>
      <Table dataSource={dataSource} bordered style={{ marginTop: 20 }}>
        <Column title="Email" dataIndex="email" key="email" />
        <Column title="Phone" dataIndex="phone" key="phone" />
        <Column
          title="Action"
          key="action"
          render={(text, record) => (
            <Space size="middle">
              {/* <a href="#" onClick={() => showEditModal(record)}>
                <EditOutlined />
              </a> */}
              <a href="#" onClick={() => showDeleteModal(record)}>
                <DeleteOutlined />
              </a>
            </Space>
          )}
        />
      </Table>
      <Modal
        title={actionType === 'edit' ? 'Edit User' : 'Add New User'}
        visible={isEditModalVisible}
        onCancel={handleEditCancel}
        onOk={form.submit}
      >
        <Form form={form} onFinish={actionType === 'edit' ? handleEdit : handleAddUser}>
          <Form.Item
            name="email"
            label="Email"
            rules={[
              {
                required: true,
                message: 'Please input the email!',
              },
            ]}
          >
            <Input />
          </Form.Item>
          <Form.Item
            name="phone"
            label="Phone"
            rules={[
              {
                required: true,
                message: 'Please input the phone number!',
              },
            ]}
          >
            <Input />
          </Form.Item>
        </Form>
      </Modal>
      <Modal
        title="Delete User"
        visible={isDeleteModalVisible}
        onCancel={handleDeleteCancel}
        onOk={handleDelete}
      >
        <p>Are you sure you want to delete this user?</p>
      </Modal>

      <Card style={{ padding: 20 }}>
        <h1>Webhooks Receiver Config</h1>
        <Button type="primary" onClick={showAddWebhooksReceiverModal}>
          Add New Receiver Webhook
        </Button>
        <Table dataSource={dataWebhookReceiverSource} bordered style={{ marginTop: 20 }}>
          <Column title="Receiver URL" dataIndex="receiverUrl" key="receiverUrl" />
          <Column title="Token" dataIndex="token" key="token" />
          <Column
            title="Action"
            key="action"
            render={(text, record) => (
              <Space size="middle">
                <a href="#" onClick={() => showDeleteWebhookReceiverModal(record)}>
                  <DeleteOutlined />
                </a>
              </Space>
            )}
          />
        </Table>
        <Modal
          title={actionType === 'edit' ? 'Edit Receiver Webhook' : 'Add New Receiver Webhook'}
          visible={isEditModalWebhookReceiverVisible}
          onCancel={handleEditWebhooksReceiverCancel}
          onOk={form.submit}
        >
          <Form form={form} onFinish={handleAddReceiverWebhook}>
            <Form.Item
              name="receiverUrl"
              label="Receiver URL"
              rules={[
                {
                  required: true,
                  message: 'Please input the receiver URL!',
                },
              ]}
            >
              <Input />
            </Form.Item>
            <Form.Item
              name="token"
              label="Token"
            >
              <Input />
            </Form.Item>
          </Form>
        </Modal>
        <Modal
          title="Delete Receiver Webhook"
          visible={isDeleteModalWebhookReceiverVisible}
          onCancel={handleDeleteReceiverCancel}
          onOk={handleDeleteReceiver}
        >
          <p>Are you sure you want to delete this receiver webhook?</p>
        </Modal>
      </Card >
    </Card>
  );
};

export default UserManagement;
