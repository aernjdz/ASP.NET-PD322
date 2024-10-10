import { useEffect, useState } from 'react';
import { httpService, BASE_URL } from '../../../api/http-services';
import { IProductItem } from '../../../interfaces/products';
import { Link } from 'react-router-dom';
import { Button, Carousel } from 'antd';
import { PlusCircleFilled, EditOutlined } from '@ant-design/icons';
import { DeleteDialog } from '../../common/delateModal/DeleteDialog';

const ProductListPage = () => {
    const [list, setList] = useState<IProductItem[]>([]);

    useEffect(() => {
        httpService.get<IProductItem[]>("/api/Products")
            .then(resp => {
                setList(resp.data);
            });
    }, []);

    const handleDelete = async (id: number) => {
        //console.log("Delete id", id);
        try {
            await httpService.delete("/api/products/" + id);
            setList(list.filter(item => item.id !== id));
        } catch {
            //toast
        }
    }

    return (
        <>
            <p className='text-center text-3xl font-bold mb-5'>Products</p>
            <Link to={"/products/create"}>
                <Button type="primary" shape="round" icon={<PlusCircleFilled />} style={{ marginTop: 10, marginBottom: 20 }} />
            </Link>

            <div className='grid md:grid-cols-3 lg:grid-cols-4 gap-10'>
                {list.map(item =>
                    <div key={item.id} className='border rounded-lg overflow-hidden shadow-lg'>
                        <Carousel arrows infinite={false}>
                            {item.images.map((image, i) => (
                                <div key={i}>
                                    <img src={`${BASE_URL}/images/300_${image}`} alt={item.name} className='w-full h-48 object-cover' />
                                </div>
                            ))}
                        </Carousel>
                        <div className='p-4'>
                            <h3 className='text-xl font-semibold mb-2'>{item.name}</h3>
                            <p className='text-teal-800 font-bold text-xl'>{item.price}<span className='text-sm'>$</span></p>
                        </div>

                        <div className='flex justify-between items-center p-2 m-2'>
                            <Link to={`/products/edit/${item.id}`} className="text-black-500 hover:text-purple-700">
                                <EditOutlined />
                            </Link>
                            <DeleteDialog title={"Notification"}
                                          description={`Are you sure you want to delete '${item.name}'?`}
                                          onSubmit={() => handleDelete(item.id)}>
                            </DeleteDialog>
                        </div>
                    </div>
                )}

            </div>
        </>
    );
}

export default ProductListPage;
