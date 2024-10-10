export interface IProductItem {
    id: number;
    name: string,
    price: string,
    images: string[],
    categoryName: string,
    categoryId: number,
}

export interface IProductCreate {
    name: string,
    price: number,
    categoryId: number,
    images: File[]|null,
}

export interface IProductEdit {
    id: number; // Matches the product ID
    name: string; // Product name
    price: number; // Product price
    categoryId: number; // ID of the category
    PreviousImages: string[]; // Changed to match the payload, representing previous image URLs
    NewImages: File[]; // New images to be uploaded, can be null or an empty array
    ImagesIds: number[]; // Array of image IDs associated with the product
}

