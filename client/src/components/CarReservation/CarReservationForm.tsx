import { useEffect, useState } from "react"
import type { ReservationFormData } from "@/lib/types"
import { InitialReservationForm } from "./ReservationSteps/InitialReservationForm/InitialReservationForm"
import { Confirmation } from "./ReservationSteps/Confirmation/Confirmation"
import { Complete } from "./ReservationSteps/Complete/Complete"

// form steps
const STEPS = {
    RESERVATION_FORM: 0,
    AVAILABILITY_CONFIRMATION: 1,
    RESERVATION_COMPLETE: 2,
  }

export default function ReservationForm() {
  // get step from URL query parameter or default to initial step
  const getStepFromUrl = (): number => {
    const searchParams = new URLSearchParams(window.location.search)
    const stepParam = searchParams.get("step")
    if (stepParam) {
      const parsedStep = Number.parseInt(stepParam, 10)
      if (!isNaN(parsedStep) && parsedStep >= 0 && parsedStep <= 2) {
        return parsedStep
      }
    }
    return STEPS.RESERVATION_FORM
  }

  const [step, setStep] = useState(getStepFromUrl())
  const [formData, setFormData] = useState<ReservationFormData>({
    startDate: undefined,
    endDate: undefined,
    pickupLocation: "",
    returnLocation: "",
    carModel: "",
    firstName: "",
    lastName: "",
    email: "",
    phone: "",
  })
  const [reservationId, setReservationId] = useState("")
  const [isLoading, setIsLoading] = useState(false)

  // update URL when step changes
  const updateStep = (newStep: number) => {
    const searchParams = new URLSearchParams(window.location.search)

    searchParams.set("step", newStep.toString())

    const newUrl = `${window.location.pathname}?${searchParams.toString()}`
    window.history.pushState({ step: newStep }, "", newUrl)
    
    setStep(newStep)
  }

  // listen for popstate events (browser back/forward buttons)
  useEffect(() => {
    const handlePopState = () => {
      setStep(getStepFromUrl())
    }

    window.addEventListener("popstate", handlePopState)
    return () => {
      window.removeEventListener("popstate", handlePopState)
    }
  }, [])

  const handleInputChange = (field: keyof ReservationFormData, value: any) => {
    setFormData((prev) => ({
      ...prev,
      [field]: value,
    }))
  }

  const handleCheckAvailability = () => {
    setIsLoading(true)
    // Simulate API call
    setTimeout(() => {
      setIsLoading(false)
      updateStep(STEPS.AVAILABILITY_CONFIRMATION)
    }, 1000)
  }

  const handleBack = () => {
    window.history.back()
  }

  // Reset form and start over
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
      phone: "",
    })
    updateStep(STEPS.RESERVATION_FORM)
  }

  const handleConfirmReservation = () => {
    setIsLoading(true)
    // simulate API call for now
    setTimeout(() => {
      const id = "1234567890"
      setReservationId(id)
      setIsLoading(false)
      updateStep(STEPS.RESERVATION_COMPLETE)
    }, 1500)
  }

  // render the appropriate step
  switch (step) {
    case STEPS.RESERVATION_FORM:
      return (
        <InitialReservationForm
          formData={formData}
          onInputChange={handleInputChange}
          onCheckAvailability={handleCheckAvailability}
          isLoading={isLoading}
        />
      )
      case STEPS.AVAILABILITY_CONFIRMATION:
      return (
        <Confirmation
          formData={formData}
          onInputChange={handleInputChange}
          onBack={handleBack}
          onConfirm={handleConfirmReservation}
          isLoading={isLoading}
        />
      )
      case STEPS.RESERVATION_COMPLETE:
      return <Complete formData={formData} reservationId={reservationId} onReset={handleReset} />
    default:
      return null
  }
}
