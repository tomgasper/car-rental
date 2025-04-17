import { useState } from "react"
import type { ReservationFormData } from "@/lib/types"
import { ReservationService } from "@/services/api/reservation"
import { getErrorMessages } from "@/services/api/error"
import { ApiError } from "@/services/api/api-error"

interface UseReservationFormReturn {
  formData: ReservationFormData
  step: number
  isLoading: boolean
  error: string | null
  reservationId: string
  handleInputChange: (field: keyof ReservationFormData, value: any) => void
  handleCheckAvailability: () => Promise<void>
  handleConfirmReservation: () => Promise<void>
  handleBack: () => void
  handleReset: () => void
  setStep: (step: number) => void
}

const changeStepParam = (step: number) => {
  const searchParams = new URLSearchParams(window.location.search)
  searchParams.set("step", step.toString())
  const newUrl = `${window.location.pathname}?${searchParams.toString()}`
  window.history.pushState({ step }, "", newUrl)
}

const getStepFromUrl = (formData: ReservationFormData): number => {
  const searchParams = new URLSearchParams(window.location.search)
  const stepParam = searchParams.get("step")
  if (stepParam) {
    const parsedStep = Number.parseInt(stepParam, 10)
    if (!isNaN(parsedStep) && parsedStep >= 0 && parsedStep <= 2) {
      if (!canAccessStep(parsedStep, formData)) {
        changeStepParam(STEPS.RESERVATION_FORM)
        return STEPS.RESERVATION_FORM
      }
      return parsedStep
    }
  }
  return STEPS.RESERVATION_FORM
}

export function useReservationForm(): UseReservationFormReturn {
  const [formData, setFormData] = useState<ReservationFormData>({
    startDate: undefined,
    endDate: undefined,
    pickupLocation: "",
    returnLocation: "",
    carModel: "",
    firstName: "",
    lastName: "",
    email: "",
    phoneNumber: "",
    totalPrice: 0,
    reservationStatus: "",
  })
  const [step, setStep] = useState(getStepFromUrl(formData))
  const [reservationId, setReservationId] = useState("")
  const [isLoading, setIsLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)

  const updateStep = (newStep: number) => {
    changeStepParam(newStep)
    setStep(newStep)
  }

  const handleInputChange = (field: keyof ReservationFormData, value: any) => {
    setFormData((prev) => ({
      ...prev,
      [field]: value,
    }))
    setError(null)
  }

  const handleCheckAvailability = async () => {
    setIsLoading(true)
    setError(null)
    try {
      const availability = await ReservationService.checkAvailability(formData);
      if (availability.length <= 0) {
        setError("No cars available for the selected dates")
        setIsLoading(false)
        return
      }
      console.log(availability)
      setFormData(prev => ({
        ...prev,
        totalPrice: availability[0].totalPrice,
      }))
      updateStep(STEPS.AVAILABILITY_CONFIRMATION)
    } catch (error) {
      if (error instanceof ApiError) {
        setError(getErrorMessages(error.details) || "Failed to check availability");
      } else {
        setError("Failed to check availability");
      }
    } finally {
      setIsLoading(false)
    }
  }

  const handleConfirmReservation = async () => {
    setIsLoading(true)
    setError(null)
    try {
      const reservation = await ReservationService.createReservation(formData)
      setReservationId(reservation.reservationId)
      setFormData(prev => ({
        ...prev,
        totalPrice: reservation.totalPrice,
        reservationStatus: reservation.reservationStatus
      }))
      updateStep(STEPS.RESERVATION_COMPLETE)
    } catch (error) {
      if (error instanceof ApiError) {
        setError(getErrorMessages(error.details) || "Failed to create reservation")
      } else {
        setError("Failed to create reservation")
      }
    } finally {
      setIsLoading(false)
    }
  }

  const handleBack = () => {
    window.history.back()
  }

  const handleReset = () => {
    setFormData({
      startDate: undefined,
      endDate: undefined,
      pickupLocation: "",
      returnLocation: "",
      carModel: "",
      firstName: "",
      lastName: "",
      email: "",
      phoneNumber: "",
      totalPrice: 0,
      reservationStatus: "",
    })
    updateStep(STEPS.RESERVATION_FORM)
  }

  return {
    formData,
    step,
    isLoading,
    error,
    reservationId,
    handleInputChange,
    handleCheckAvailability,
    handleConfirmReservation,
    handleBack,
    handleReset,
    setStep,
  }
}

const STEPS = {
  RESERVATION_FORM: 0,
  AVAILABILITY_CONFIRMATION: 1,
  RESERVATION_COMPLETE: 2,
} as const

const canAccessStep = (step: number, formData: ReservationFormData): boolean => {
  switch (step) {
    case STEPS.RESERVATION_FORM:
      return true;
    case STEPS.AVAILABILITY_CONFIRMATION:
      // initial form data and total price must be set
      return !!(
        formData.startDate &&
        formData.endDate &&
        formData.pickupLocation &&
        formData.returnLocation &&
        formData.carModel
      );
    case STEPS.RESERVATION_COMPLETE:
      // personal info and reservation status must be set
      return !!(
        formData.firstName &&
        formData.lastName &&
        formData.email &&
        formData.phoneNumber
      );
    default:
      return false;
  }
}