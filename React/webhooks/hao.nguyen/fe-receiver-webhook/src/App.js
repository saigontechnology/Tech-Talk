import React, { useState, useEffect } from 'react';
import { Table, Space, Button, Modal, Form, Input, Card, Divider, Tag } from 'antd';
import { DeleteOutlined } from '@ant-design/icons';
import axios from 'axios';

const { Column } = Table;

const ReceiverWebhooksManagement = () => {
  const [dataSource, setDataSource] = useState([]);
  const [webhooksLogs, setWebhookLogs] = useState([]);
  const [form] = Form.useForm();
  const [isEditModalVisible, setIsEditModalVisible] = useState(false);
  const [isDeleteModalVisible, setIsDeleteModalVisible] = useState(false);
  const [selectedReceiverWebhook, setSelectedReceiverWebhook] = useState(null);
  const [actionType, setActionType] = useState(null);

  useEffect(() => {
    // Initial fetch
    fetchWebhookLogs();

    // Set up an interval to fetch data every 5 seconds
    const intervalId = setInterval(() => {
      fetchWebhookLogs();
    }, 2000); // 5000 milliseconds = 5 seconds

    // Clean up the interval when the component unmounts
    return () => {
      clearInterval(intervalId);
    };
  }, []);

  const fetchWebhookLogs = () => {
    axios.get('http://localhost:9000/api/client/webhooks-logs')
      .then((response) => {
        setWebhookLogs(response.data);
      })
      .catch((error) => {
        console.error('Error fetching webhooks logs:', error);
      });
  };

  const deleteAllWebhookLogs = () => {
    axios.delete('http://localhost:9000/api/client/webhooks-logs')
      .then((response) => {
        setWebhookLogs([]);
      })
      .catch((error) => {
        console.error('Error fetching webhooks logs:', error);
      });
  };

  const showAddModal = () => {
    setIsEditModalVisible(true);
    setActionType('add');
  }

  const showDeleteModal = (receiverWebhook) => {
    setSelectedReceiverWebhook(receiverWebhook);
    setIsDeleteModalVisible(true);
  };

  const handleEditCancel = () => {
    setIsEditModalVisible(false);
    setSelectedReceiverWebhook(null);
  };

  const handleDeleteCancel = () => {
    setIsDeleteModalVisible(false);
    setSelectedReceiverWebhook(null);
  };

  const handleDelete = () => {
    axios.delete(`http://localhost:9000/api/webhooks-receiver/${selectedReceiverWebhook.id}`)
      .then(() => {
        const updatedDataSource = dataSource.filter(
          (receiverWebhook) => receiverWebhook.id !== selectedReceiverWebhook.id
        );
        setDataSource(updatedDataSource);
        setIsDeleteModalVisible(false);
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
        setDataSource([...dataSource, newReceiverWebhook]);
        form.resetFields();
        setIsEditModalVisible(false);
      })
      .catch((error) => {
        console.error('Error adding receiver webhook:', error);
      });
  };

  return (
    <div style={{ padding: 20 }}>
      {/* <Card style={{ padding: 20 }}>
        <h1>Webhooks Receiver Config</h1>
        <Button type="primary" onClick={showAddModal}>
          Add New Receiver Webhook
        </Button>
        <Table dataSource={dataSource} bordered style={{ marginTop: 20 }}>
          <Column title="Receiver URL" dataIndex="receiverUrl" key="receiverUrl" />
          <Column title="Token" dataIndex="token" key="token" />
          <Column
            title="Action"
            key="action"
            render={(text, record) => (
              <Space size="middle">
                <a href="#" onClick={() => showDeleteModal(record)}>
                  <DeleteOutlined />
                </a>
              </Space>
            )}
          />
        </Table>
        <Modal
          title={actionType === 'edit' ? 'Edit Receiver Webhook' : 'Add New Receiver Webhook'}
          visible={isEditModalVisible}
          onCancel={handleEditCancel}
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
          visible={isDeleteModalVisible}
          onCancel={handleDeleteCancel}
          onOk={handleDelete}
        >
          <p>Are you sure you want to delete this receiver webhook?</p>
        </Modal>
      </Card > */}
      http://localhost:9000/api/client/webhooks-logs
      <Card style={{ padding: 20, marginTop: 20 }}>
        <Button style={{ marginLeft: 10 }} onClick={fetchWebhookLogs}>
          Fetch Webhooks Logs
        </Button>
        <Button style={{ marginLeft: 10 }} danger onClick={deleteAllWebhookLogs}>
          Delete all Webhooks Logs
        </Button>
        <Divider orientation="left">Logs</Divider>
        <Space size={[0, 8]} style={{ display: 'flex', flexDirection: 'column', alignItems: 'start' }}>
          {
            webhooksLogs?.map(item => {
              return <Tag color="#2db7f5" style={{ fontSize: 15 }}>{item?.createdAt} - {item?.id} - {item?.content}</Tag>
            })
          }
        </Space>
      </Card>
    </div>
  );
};

export default ReceiverWebhooksManagement;
