export interface Location {
    id: string
    name: string
  }
  
  export interface TeslaModel {
    id: string
    name: string
    image: string
    pricePerDay: number
    description: string
  }
  
  export interface ReservationFormData {
    startDate: Date | undefined
    endDate: Date | undefined
    pickupLocation: string
    returnLocation: string
    carModel: string
    firstName: string
    lastName: string
    email: string
    phoneNumber: string
  }
  
  export interface ReservationSummary {
    reservationId: string
    formData: ReservationFormData
    totalPrice: number
    days: number
  }
  