export interface Book {
  id: string;
  title: string;
  author: string;
  description: string | null;
  price: number;
  stockQuantity: number;
  genreId: string;
}

export interface CreateBookRequest {
  title: string;
  author: string;
  description: string | null;
  price: number;
  stockQuantity: number;
  genreId: string;
}

// API не позволяет менять остаток на складе при обновлении книги.
export interface UpdateBookRequest {
  title: string;
  author: string;
  description: string | null;
  price: number;
  genreId: string;
}
