export class ApiError extends Error {
    details: any[];
    status?: number;
  
    constructor(message: string, details?: any[], status?: number) {
      super(message);
      this.name = 'ApiError';
      this.details = details || [];
      this.status = status;
      
      // Ensures proper prototype chain for instanceof checks
      Object.setPrototypeOf(this, ApiError.prototype);
    }
  
    static fromResponse(response: Response, data: any): ApiError {
      const message = data.title || 'API Error';
      const details = data.errors || [];
      return new ApiError(message, details, response.status);
    }
  }