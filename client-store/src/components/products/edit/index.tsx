import { useState, useEffect } from "react";
import { useNavigate, useParams, Link } from "react-router-dom";
import { httpService, BASE_URL } from '../../../api/http-services';
import { Button, Form, Modal, Input, Upload, UploadFile, Space, InputNumber, Select } from "antd";
import { RcFile, UploadChangeParam } from "antd/es/upload";
import { PlusOutlined } from '@ant-design/icons';
import { IProductEdit } from "../../../interfaces/products";
import { ICategoryName } from '../../../interfaces/categories';

const ProductEditPage = () => {
    const { id } = useParams(); // Get the product ID from URL parameters
    const navigate = useNavigate(); // For navigation
    const [form] = Form.useForm<IProductEdit>(); // Form instance
    const [fileList, setFileList] = useState<UploadFile[]>([]); // State for uploaded files

    const [previewOpen, setPreviewOpen] = useState<boolean>(false);
    const [previewImage, setPreviewImage] = useState<string>('');
    const [previewTitle, setPreviewTitle] = useState<string>('');
    const [categories, setCategories] = useState<ICategoryName[]>([]); // Categories state

    // Fetch categories on component mount
    useEffect(() => {
        const fetchCategories = async () => {
            try {
                const response = await httpService.get<ICategoryName[]>("/api/Categories/names");
                setCategories(response.data);
            } catch (error) {
                console.error("Error fetching categories: ", error);
            }
        };
        fetchCategories();
    }, []);

    // Fetch product details on component mount
    useEffect(() => {
        const fetchProductDetails = async () => {
            try {
                const response = await httpService.get<IProductEdit>(`/api/products/${id}`);
                const { data } = response;

                console.log(data);
                form.setFieldsValue(data);

                // Populate previous images for the upload component
                if (Array.isArray(data.productImages) && data.productImages.length > 0) {
                    const previousFiles = data.productImages.map((imageObj) => ({
                        uid: imageObj.id.toString(), // Unique ID for the Upload component
                        name: imageObj.image,
                        status: 'done',
                        url: `${BASE_URL}/images/300_${imageObj.image}`, // URL to display
                    }));
                    setFileList(previousFiles); // Set previous files to fileList
                }
            } catch (error) {
                console.error("Error fetching product details: ", error);
            }
        };
        fetchProductDetails();
    }, [form, id]);

    const onSubmit = async (values: IProductEdit) => {
        try {
            const formattedPrice = parseFloat(values.price.toString()).toFixed(2); // Ensure two decimal places

            const productData = {
                id: id,
                name: values.name,
                price: formattedPrice, // Use formatted price here
                categoryId: values.categoryId,
                previousImages: fileList.map(file => ({
                    id: file.uid,
                    file: file.name,
                    priority: 0 // Assuming you want a default priority
                })),
                newImages: fileList.filter(file => file.originFileObj).map(file => file.originFileObj)
            };

            console.log("Product data being sent:", productData); // Debugging output

            const response = await httpService.put(`/api/products`, productData, {
                headers: { "Content-Type": "application/json" } // Ensure it's sent as JSON
            });
            console.log("Product updated: ", response.data);
            navigate('/'); // Navigate to the product list or another page
        } catch (error) {
            console.error("Error updating product: ", error);
        }
    };

    return (
        <>
            <p className="text-center text-3xl font-bold mb-7">Edit Product</p>
            <Form form={form} onFinish={onSubmit} labelCol={{ span: 6 }} wrapperCol={{ span: 14 }}>
                <Form.Item
                    name="name"
                    label="Name"
                    hasFeedback
                    rules={[{ required: true, message: 'Please provide a valid product name.' }]}>
                    <Input placeholder='Type product name' />
                </Form.Item>

                <Form.Item
                    name="price"
                    label="Price"
                    hasFeedback
                    rules={[{ required: true, message: 'Please enter product price.' }]}>
                    <InputNumber addonAfter="$" placeholder='0.00' />
                </Form.Item>

                <Form.Item
                    name="categoryId"
                    label="Category"
                    hasFeedback
                    rules={[{ required: true, message: 'Please choose a category.' }]}>
                    <Select placeholder="Select a category">
                        {categories.map(category => (
                            <Select.Option key={category.id} value={category.id}>
                                {category.name}
                            </Select.Option>
                        ))}
                    </Select>
                </Form.Item>

                <Form.Item
                    name="images"
                    label="Photo"
                    valuePropName="Image"
                    rules={[{ required: true, message: "Please choose a photo for the product." }]}
                    getValueFromEvent={(e: UploadChangeParam) => e?.fileList.map(file => file.originFileObj)}>
                    <Upload
                        beforeUpload={() => false}
                        accept="image/*"
                        maxCount={10}
                        listType="picture-card"
                        multiple
                        fileList={fileList} // Bind the fileList to the Upload component
                        onChange={({ fileList: newFileList }) => setFileList(newFileList)} // Update fileList on change
                        onPreview={(file: UploadFile) => {
                            if (!file.url && !file.preview) {
                                file.preview = URL.createObjectURL(file.originFileObj as RcFile);
                            }

                            setPreviewImage(file.url || (file.preview as string));
                            setPreviewOpen(true);
                            setPreviewTitle(file.name || file.url!.substring(file.url!.lastIndexOf('/') + 1));
                        }}
                    >
                        <div>
                            <PlusOutlined />
                            <div style={{ marginTop: 8 }}>Upload</div>
                        </div>
                    </Upload>
                </Form.Item>

                <Form.Item wrapperCol={{ span: 10, offset: 10 }}>
                    <Space>
                        <Link to="/products">
                            <Button htmlType="button" className='text-white bg-gradient-to-br from-red-400 to-purple-600 font-medium rounded-lg px-5'>Cancel</Button>
                        </Link>
                        <Button htmlType="submit" className='text-white bg-gradient-to-br from-green-400 to-blue-600 font-medium rounded-lg px-5'>Update</Button>
                    </Space>
                </Form.Item>
            </Form>

            <Modal open={previewOpen} title={previewTitle} footer={null} onCancel={() => setPreviewOpen(false)}>
                <img alt="example" style={{ width: '100%' }} src={previewImage} />
            </Modal>
        </>
    );
};

export default ProductEditPage;
