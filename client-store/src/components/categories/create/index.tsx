import { useState } from 'react';
import { Form, Input, Button, Modal, Upload, UploadFile, Space } from 'antd';
import { useNavigate, Link } from 'react-router-dom';
import { httpService } from '../../../api/http-services';
import { RcFile, UploadChangeParam } from "antd/es/upload";
import { PlusOutlined } from '@ant-design/icons';
import { ICategoryCreate, IUploadedFile } from '../../../interfaces/categories';
import Loader from '../../common/loader/Loader';

const CategoryCreatePage = () => {

    const navigate = useNavigate();
    const [form] = Form.useForm<ICategoryCreate>();
    const [loading, setLoading] = useState<boolean>(false);

    const [previewOpen, setPreviewOpen] = useState<boolean>(false);
    const [previewImage, setPreviewImage] = useState('');
    const [previewTitle, setPreviewTitle] = useState('');

    const onSubmit = async (values: ICategoryCreate) => {
        setLoading(true);
        try {
            console.log("Send Data", values);

            httpService.post<ICategoryCreate>("/api/categories", values, {
                headers: { "Content-Type": "multipart/form-data" }
            }).then(resp => {
                console.log("Create category", resp.data);
                navigate('/');
            });
        } catch (error) {
            console.log("Error: ", error);
        } finally {
            setLoading(false);
        }
    };

    return (
        <> {loading ? (
            <Loader />
        ) : (
            <>
                <p className="text-center text-3xl font-bold mb-7">Create Category</p>
                <Form form={form} onFinish={onSubmit} labelCol={{ span: 6 }} wrapperCol={{ span: 14 }}>
                    <Form.Item name="name" label="Name" hasFeedback
                               rules={[{ required: true, message: 'Please provide a valid category name.' }]}>
                        <Input placeholder='Type category name' />
                    </Form.Item>

                    <Form.Item name="description" label="Description" hasFeedback
                               rules={[{ required: true, message: 'Please enter some description.' }]}>
                        <Input.TextArea placeholder='Type some description' rows={4} />
                    </Form.Item>

                    <Form.Item name="Image" label="Photo" valuePropName="Image"
                               getValueFromEvent={(e: UploadChangeParam) => {
                                   const image = e?.fileList[0] as IUploadedFile;
                                   return image?.originFileObj;
                               }}
                               rules={[{ required: true, message: "Please choose a photo for the category." }]}>
                        <Upload beforeUpload={() => false} accept="image/*"
                                onPreview={(file: UploadFile) => {
                                    if (!file.url && !file.preview) {
                                        file.preview = URL.createObjectURL(file.originFileObj as RcFile);
                                    }

                                    setPreviewImage(file.url || (file.preview as string));
                                    setPreviewOpen(true);
                                    setPreviewTitle(file.name || file.url!.substring(file.url!.lastIndexOf('/') + 1));
                                }}
                                listType="picture-card"
                                maxCount={1}>
                            <div>
                                <PlusOutlined />
                                <div style={{ marginTop: 8 }}>Upload</div>
                            </div>
                        </Upload>
                    </Form.Item>

                    <Form.Item wrapperCol={{ span: 10, offset: 10 }}>
                        <Space>
                            <Link to={"/"}>
                                <Button htmlType="button" className='text-white bg-gradient-to-br from-red-400 to-purple-600 font-medium rounded-lg px-5'>Cancel</Button>
                            </Link>
                            <Button htmlType="submit" className='text-white bg-gradient-to-br from-green-400 to-blue-600 font-medium rounded-lg px-5'>Create</Button>
                        </Space>
                    </Form.Item>
                </Form>

                <Modal open={previewOpen} title={previewTitle} footer={null} onCancel={() => setPreviewOpen(false)}>
                    <img alt="example" style={{ width: '100%' }} src={previewImage} />
                </Modal>
            </>
        )}
        </>
    );
};

export default CategoryCreatePage;