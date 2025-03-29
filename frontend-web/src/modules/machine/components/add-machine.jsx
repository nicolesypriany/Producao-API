import RegisterModal from "@/modules/core/components/modal";
import { Button, Flex, Group, TextInput } from "@mantine/core";
import { useForm } from "@mantine/form";
import { createMachine as createMachineService } from "../services";

export const AddMachine = () => {
  const { key, getInputProps, onSubmit } = useForm({
    mode: "uncontrolled",
    initialValues: {
      machineName: "",
      brand: "",
    },
  });

  return (
    <RegisterModal title={"Cadastrar máquina"}>
      <form
        onSubmit={onSubmit(async (data) => await createMachineService(data))}
      >
        <Flex direction="column" gap={4}>
          <TextInput
            label="Nome da máquina"
            placeholder="Digite o nome da máquina"
            key={key("machineName")}
            {...getInputProps("machineName")}
          />

          <TextInput
            label="Marca"
            placeholder="Digite o nome da marca"
            key={key("brand")}
            {...getInputProps("brand")}
          />

          <Group mt="lg" justify="flex-end">
            <Button type="submit" variant="filled" w="30%">
              Salvar
            </Button>
          </Group>
        </Flex>
      </form>
    </RegisterModal>
  );
};
