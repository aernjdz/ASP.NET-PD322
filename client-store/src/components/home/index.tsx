import { useEffect, useState } from 'react';
import { httpService, BASE_URL } from '../../api/http-services';
import { ICategoryItem } from '../../interfaces/categories';
import { Link } from 'react-router-dom';
import { DeleteDialog } from '../common/delateModal/DeleteDialog';

const HomePage = () => {
    const [list, setList] = useState<ICategoryItem[]>([]);

    useEffect(() => {
        httpService.get<ICategoryItem[]>("/api/Categories")
            .then(resp => {
                setList(resp.data);
            });
    }, []);

    const handleDelete = (id: number) => {
        try {
            httpService.delete(`/api/categories/` + id);
            setList(list.filter(i => i.id != id));
            console.log(`The category with id ${id} has been deleted`);
        } catch (error) {
            console.log("Error: ", error);
        }
    }

    return (
        <>
            <p className='text-center text-3xl font-bold mb-5'>Categories</p>
            <Link to={"/create"}>
                <button className="mb-4 text-white font-bold bg-gray-500 hover:bg-gray-600 py-1 px-4 rounded">
                    Add
                </button>
            </Link>

            <div className='grid md:grid-cols-3 lg:grid-cols-4 gap-6'>
                {list.map(x =>
                    <div key={x.id} className='border rounded-lg overflow-hidden shadow-lg'>
                        <img src={`${BASE_URL}/images/${x.image}`}
                             onError={(e) => { (e.currentTarget.src = `${BASE_URL}/images/1200_${x.image}`) }}
                             alt={x.name} className='w-full h-48 object-cover' />
                        <div className='p-4'>
                            <h3 className='text-xl font-semibold mb-2'>{x.name}</h3>
                            <p className='text-gray-700'>{x.description}</p>

                            <div className='flex justify-between items-center p-2 mt-6'>
                                <Link to={`/edit/${x.id}`} className="text-black-500 hover:text-purple-700">
                                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" className="size-5">
                                        <path stroke-linecap="round" stroke-linejoin="round" d="m16.862 4.487 1.687-1.688a1.875 1.875 0 1 1 2.652 2.652L6.832 19.82a4.5 4.5 0 0 1-1.897 1.13l-2.685.8.8-2.685a4.5 4.5 0 0 1 1.13-1.897L16.863 4.487Zm0 0L19.5 7.125" />
                                    </svg>
                                </Link>

                                <DeleteDialog title={"Notification"}
                                              description={`Are you sure you want to delete '${x.name}'?`}
                                              onSubmit={() => handleDelete(x.id)}>
                                </DeleteDialog>
                            </div>

                        </div>
                    </div>
                )}
            </div>
        </>
    );
}

export default HomePage;
